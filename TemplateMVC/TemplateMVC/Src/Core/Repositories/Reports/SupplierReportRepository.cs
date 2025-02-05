using System.Data;
using Dapper;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Models.Suppliers;

namespace TemplateMVC.Core.Repositories.Reports;

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