using TemplateMongoDbApi.Common.Exceptions;
using TemplateMongoDbApi.Common.Helpers;
using TemplateMongoDbApi.Core.Models.Products;
using TemplateMongoDbApi.Core.Repositories;
using TemplateMongoDbApi.Core.Repositories.Products;
using TemplateMongoDbApi.Core.Repositories.Suppliers;

namespace TemplateMongoDbApi.Core.Services.Products;

public class ProductService
{
    private readonly ProductRepository _repository;
    private readonly ProductImageRepository _imageRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly SupplierRepository _supplierRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly FileUploader _imageUploader;
    private readonly string _uploadDirectory;

    public ProductService(ProductRepository repository, ProductImageRepository imageRepository, 
        CategoryRepository categoryRepository, SupplierRepository supplierRepository,
        IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _supplierRepository = supplierRepository;
        _imageRepository = imageRepository;
        _contextAccessor = contextAccessor;
        _configuration = configuration;

        _uploadDirectory = _configuration["UploadsDirectory"] + "/Products";
        _imageUploader = new FileUploader(_uploadDirectory, FileExtensions.Images, 5 * CapacityUnit.MegaByte);;
    }

    public async Task CreateProduct(ProductRequest request)
    {
        if (request is null)
        {
            throw new BadRequestException("The product request cannot be null. Please provide: Name, Code, Price and other data.");
        }
        if (await _repository.ExistsRecord("Code", request.Code))
        {
            throw new ConflictException($"Product with code: '{request.Code}' already exists.");
        }
        var product = new Product()
        {
            Category = await _categoryRepository.GetByIdAsync(request!.CategoryId),
            Supplier = await _supplierRepository.GetByIdAsync(request!.SupplierId),
            ProductName = request.ProductName,
            Code = request.Code,
            UnitPrice = request.UnitPrice,
            Description = request.Description,
        };
        await _repository.CreateAsync(product);
    }

    public async Task UpdateProduct(ProductRequest request, string productId)
    {
        if (request is null)
        {
            throw new BadRequestException("The product request cannot be null. Please provide: Name, Code, Price and other data.");
        }
        var product = await _repository.GetByIdAsync(productId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{productId}' not found");
        }
        product.Category = await _categoryRepository.GetByIdAsync(request!.CategoryId);
        product.Supplier = await _supplierRepository.GetByIdAsync(request!.SupplierId);
        product.ProductName = request.ProductName;
        product.Code = request.Code;
        product.UnitPrice = request.UnitPrice;
        product.Description = request.Description;
        product.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(product);
    }

    public async Task<Pagination<ProductResponse>> GetAllProducts(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var products = await _repository.GetAllDataAsync(param.PageSize, param.PageIndex);
        var productResponses = ProductMapper.ToResponseList(products);
        var pagination = new Pagination<ProductResponse>(productResponses, count, param.PageIndex, param.PageSize, _contextAccessor);
        return pagination;
    }

    public async Task<ProductResponse> GetProductByUniqueId(string productId)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{productId}' not found");
        }
        var productResponse = ProductMapper.ToResponse(product);
        return productResponse;
    }

    public async Task DeleteProduct(string productId)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{productId}' not found");
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
                    throw new BadRequestException("Invalid CSV format. Each line must contain ProductName,Code,UnitPrice,CategoryName,SupplierName.");
                }
                // verify csv format
                if (!float.TryParse(data[2], out float unitPrice))
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
                    Category = await _categoryRepository.GetByNameAsync(data[3]),
                    Supplier = await _supplierRepository.GetByNameAsync(data[4]) 
                };
                products.Add(product);
            }
        }
        return products;
    }

    public async Task UploadProductImages(string productId, IFormFileCollection files) 
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{productId}' not found");
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

    public async Task<IEnumerable<ProductImage>> GetProductImages(string productId)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product is null)
        {
            throw new NotFoundException($"Product with ID '{product}' not found");
        }
        
        var images = await _imageRepository.GetAllByProductAsync(productId);
        return images;
    }
}