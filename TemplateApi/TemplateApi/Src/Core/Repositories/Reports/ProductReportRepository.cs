using System.Data;
using Dapper;
using TemplateApi.Core.Models.Reports;

namespace TemplateApi.Core.Repositories.Reports;

public class ProductReportRepository
{
    private readonly IDbConnection _dapper;

    public ProductReportRepository(IDbConnection dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<ProductReport>> GetAllAsync(ReportFilter filter)
    {
        var sql = "SELECT * FROM ViewProductReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
        return await _dapper.QueryAsync<ProductReport>(sql, new { StartDate = filter.StartDate, EndDate = filter.EndDate });
    }
}