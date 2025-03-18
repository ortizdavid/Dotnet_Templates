using System.Data;
using Dapper;
using TemplateRabbitMQApi.Core.Models.Reports;
using TemplateRabbitMQApi.Core.Models.Suppliers;

namespace TemplateRabbitMQApi.Core.Repositories.Reports;

public class SupplierReportRepository
{
    private readonly IDbConnection _dapper;
    
    public SupplierReportRepository(IDbConnection dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<SupplierReport>> GetAllAsync(ReportFilter filter)
    {
        var sql = "SELECT * FROM ViewSupplierReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
        return await _dapper.QueryAsync<SupplierReport>(sql, new { StartDate = filter.StartDate, EndDate = filter.EndDate});
    }
}