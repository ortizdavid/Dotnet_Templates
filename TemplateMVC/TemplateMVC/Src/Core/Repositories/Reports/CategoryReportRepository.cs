using System.Data;
using Dapper;
using TemplateMVC.Common.Helpers;
using TemplateMVC.Core.Models.Reports;

namespace TemplateMVC.Core.Repositories.Reports;

public class CategoryReportRepository
{
    private readonly IDbConnection _dapper;

    public CategoryReportRepository(IDbConnection dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<CategoryReport>> GetAllAsync(ReportFilter filter)
    {
        var sql = "SELECT * FROM ViewCategoryReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
        return await _dapper.QueryAsync<CategoryReport>(sql, new { StartDate = filter.StartDate, EndDate = filter.EndDate }); 
    }
}