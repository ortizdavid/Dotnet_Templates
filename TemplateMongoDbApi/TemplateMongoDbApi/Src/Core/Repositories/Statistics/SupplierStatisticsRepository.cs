using TemplateMongoDbApi.Core.Models.Statistics;

namespace TemplateMongoDbApi.Core.Repositories.Statistics;

public class SupplierStatisticsRepository
{
    public SupplierStatisticsRepository()
    {
    }

    public async Task<IEnumerable<SupplierTopSuppliers>> GetTopSupplierCategoriesAsync()
    {
        var sql = "SELECT * FROM ViewSupplierTopSuppliers;";
        return null;
    }  
}