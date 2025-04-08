using TemplateMongoDbApi.Core.Models.Statistics;

namespace TemplateMongoDbApi.Core.Repositories.Statistics;

public class ProductStatisticsRepository
{

    public ProductStatisticsRepository()
    {
    }

    public Task<IEnumerable<ProductTotalPriceByCategories>> GetProductTotalPriceByCategoriesAsync()
    {
        var sql = "SELECT * FROM ViewProductTotalPriceByCategories;";
        return null;
    }

    public async Task<IEnumerable<ProductTotalPriceBySuppliers>> GetProductTotalPriceBySuppliersAsync()
    {
        var sql = "SELECT * FROM ViewProductTotalPriceBySuppliers;";
        return null;
    }
}
