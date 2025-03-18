using TemplateMVC.Core.Models.Statistics;
using TemplateMVC.Core.Repositories.Statistics;

namespace TemplateMVC.Core.Services.Statistics;

public class ProductStatisticsService
{
    private readonly ProductStatisticsRepository _repository;

    public ProductStatisticsService(ProductStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductTotalPriceByCategories>> GetProductTotalPriceByCategories()
    {
        return await _repository.GetProductTotalPriceByCategoriesAsync();
    }

    public async Task<IEnumerable<ProductTotalPriceBySuppliers>> GetProductTotalPriceBySuppliers()
    {
        return await _repository.GetProductTotalPriceBySuppliersAsync();
    }
}