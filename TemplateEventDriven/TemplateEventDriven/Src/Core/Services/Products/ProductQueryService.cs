using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Repositories;
using TemplateEventDriven.Core.Repositories.Products;

namespace TemplateEventDriven.Core.Services.Products;

public class ProductQueryService
{
    private readonly ProductRepository _repository;
    private readonly ProductImageRepository _imageRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public ProductQueryService(ProductRepository repository, ProductImageRepository imageRepository, 
        IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _imageRepository = imageRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task<Pagination<ProductData>> GetAllProducts(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var products = await _repository.GetAllDataAsync(param.PageSize, param.PageIndex);
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