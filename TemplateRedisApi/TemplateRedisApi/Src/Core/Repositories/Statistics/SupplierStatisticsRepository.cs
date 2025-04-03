using System.Data;
using Dapper;
using TemplateRedisApi.Core.Models.Statistics;

namespace TemplateRedisApi.Core.Repositories.Statistics;

public class SupplierStatisticsRepository
{
    private readonly IDbConnection _dapper;

    public SupplierStatisticsRepository(IDbConnection dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<SupplierTopSuppliers>> GetTopSupplierCategoriesAsync()
    {
        var sql = "SELECT * FROM ViewSupplierTopSuppliers;";
        return await _dapper.QueryAsync<SupplierTopSuppliers>(sql);
    }  
}