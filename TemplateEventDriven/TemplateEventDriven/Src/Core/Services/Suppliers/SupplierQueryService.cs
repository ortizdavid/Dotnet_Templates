using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Models.Suppliers;
using TemplateEventDriven.Core.Repositories;
using TemplateEventDriven.Core.Repositories.Suppliers;

namespace TemplateEventDriven.Core.Services.Suppliers;

public class SupplierQueryService
{
    private readonly SupplierRepository _repository;
    private readonly ProductRepository _productRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public SupplierQueryService(SupplierRepository repository, ProductRepository productRepository, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _productRepository = productRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task<Pagination<Supplier>> GetAllSuppliers(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var suppliers = await _repository.GetAllAsync(param.PageSize, param.PageIndex);
        var pagination = new Pagination<Supplier>(suppliers, count, param.PageIndex, param.PageSize, _contextAccessor);
        return pagination;
    }

    public async Task<Supplier> GetSupplierByUniqueId(Guid uniqueId)
    {
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }
        return supplier;
    }

    public async Task<IEnumerable<Product>> GetSupplierProducts(Guid uniqueId)
    {
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }
        var products = await _productRepository.GetAllBySupplierAsync(supplier.SupplierId);
        return products;
    }
}