using System.Data;
using Dapper;
using TemplateEventDriven.Core.Models.Statistics;

namespace TemplateEventDriven.Core.Repositories.Statistics;

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