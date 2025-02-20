using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Repositories;
using TemplateMVC.Core.Repositories.Products;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Services.Products;

public class ProductService
{
    private readonly ProductRepository _repository;
    private readonly ProductImageRepository _imageRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly FileUploader _imageUploader;
    private readonly string _uploadDirectory;

    public ProductService(ProductRepository repository, ProductImageRepository imageRepository, 
        IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _repository = repository;
        _imageRepository = imageRepository;
        _contextAccessor = contextAccessor;
        _configuration = configuration;

        _uploadDirectory = _configuration["UploadsDirectory"] + "/Products";
        _imageUploader = new FileUploader(_uploadDirectory, FileExtensions.Images, 5 * CapacityUnit.MEGA_BYTE);;
    }

    public async Task CreateProduct(CreateProductViewModel viewModel)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("The product viewModel cannot be null. Please provide: Name, Code, Price and other data.");
        }
        if (await _repository.ExistsRecord("Code", viewModel.Code))
        {
            throw new ConflictException($"Product with code: '{viewModel.Code}' already exists.");
        }
        var product = new Product()
        {
            CategoryId = viewModel.CategoryId,
            SupplierId = viewModel.SupplierId,
            ProductName = viewModel.ProductName,
            Code = viewModel.Code,
            UnitPrice = viewModel.UnitPrice,
            Description = viewModel.Description,
        };
        await _repository.CreateAsync(product);
    }

    public async Task UpdateProduct(UpdateProductViewModel viewModel, Guid uniqueId)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("The product viewModel cannot be null. Please provide: Name, Code, Price and other data.");
        }
        var product = await _repository.GetByUniqueIdAsync(uniqueId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{uniqueId}' not found");
        }
        product.CategoryId = viewModel.CategoryId;
        product.SupplierId = viewModel.SupplierId;
        product.ProductName = viewModel.ProductName;
        product.Code = viewModel.Code;
        product.UnitPrice = viewModel.UnitPrice;
        product.Description = viewModel.Description;
        product.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(product);
    }

    public async Task<Pagination<ProductData>> GetAllProducts(PaginationParam param, SearchFilter filter)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var products = await _repository.GetAllDataSortedAsync(param.PageSize, param.PageIndex, filter.SearchString, filter.SortOrder);
        var pagination = new Pagination<ProductData>(products, count, param.PageIndex, param.PageSize, _contextAccessor);
        return pagination;
    }

    public async Task<ProductData> GetProductByUniqueId(Guid uniqueId)
    {
        var product = await _repository.GetDataByUniqueIdAsync(uniqueId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{uniqueId}' not found");
        }
        return product;
    }

    public async Task DeleteProduct(Guid uniqueId)
    {
        var product = await _repository.GetByUniqueIdAsync(uniqueId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{uniqueId}' not found");
        }
        await _repository.DeleteAsync(product);
    }

    public async Task ImportProductsCSV(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new BadRequestException("No file selected.");
        }
        if (Path.GetExtension(formFile.FileName).ToLower() != ".csv")
        {
            throw new BadRequestException("Invalid file format. Please upload a CSV file.");
        }
        var products = await ParseCSV(formFile);
        await _repository.CreateBatchAsync(products);
    }

    private async Task<IEnumerable<Product>> ParseCSV(IFormFile formFile)
    {
        var products = new List<Product>();
        using (StreamReader reader = new StreamReader(formFile.OpenReadStream()))
        {
            // Skip the header line
            await reader.ReadLineAsync();
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var data = line.Split(',');
                var productCode = data[1];
                // verify number of fields
                if (data.Length != 5)
                {
                    throw new BadRequestException("Invalid CSV format. Each line must contain ProductName,Code,UnitPrice,CategoryId,SupplierId.");
                }
                // verify csv format
                if (!float.TryParse(data[2], out float unitPrice) || !int.TryParse(data[3], out int categoryId) || !int.TryParse(data[4], out int supplierId))
                {
                    throw new BadRequestException("Invalid CSV format. UnitPrice and CategoryId and SupplierId must be numeric.");
                }
                //verify if exists
                if (await _repository.ExistsRecord("Code", productCode))
                {
                    throw new ConflictException($"Product code '{productCode}' already exists");
                }
                var product = new Product
                {
                    ProductName = data[0],
                    Code = productCode,
                    UnitPrice = decimal.Parse(data[2]),
                    CategoryId = int.Parse(data[3]),
                    SupplierId = int.Parse(data[4]) 
                };
                products.Add(product);
            }
        }
        return products;
    }

    public async Task UploadProductImages(Guid uniqueId, IFormFileCollection files) 
    {
        var product = await _repository.GetByUniqueIdAsync(uniqueId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{uniqueId}' not found");
        }
        if (files == null || files.Count == 0)
        {
            throw new BadRequestException("No files selected.");
        }
        if (files.Count > 4) 
        {
            throw new BadRequestException("Only 4 images are allowed: front, back, left and right.");
        }

        var imagesInfo = await _imageUploader.UploadMultipleFiles(files);
        var productImages = new List<ProductImage>();// Create a list to hold ProductImage entities

        // Iterate over imagesInfo using a foreach loop
        foreach (var imageInfo in imagesInfo)
        {
            var productImage = new ProductImage
            {
                ProductId = product.ProductId,
                FileName = imageInfo.FinalName,
                UploadDir = _uploadDirectory
            };
            productImages.Add(productImage); 
        }

        await _imageRepository.CreateBatchAsync(productImages);
    }

    public async Task<IEnumerable<ProductImage>> GetProductImages(Guid uniqueId)
    {
        var product = await _repository.GetByUniqueIdAsync(uniqueId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{uniqueId}' not found");
        }
        
        var images = await _imageRepository.GetAllByProductAsync(product.ProductId);
        return images;
    }
}