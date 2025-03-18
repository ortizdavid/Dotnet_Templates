using System.Data;
using Dapper;
using TemplateMVC.Common.Helpers;
using TemplateMVC.Core.Models.Reports;

namespace TemplateMVC.Core.Repositories.Reports;

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

    public async Task<IEnumerable<ProductReport>> GetAllByCategoryAsync(ProductReportFilter filter)
    {
        var sql = "SELECT * FROM ViewProductReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate AND CategoryId = @CategoryId";
        return await _dapper.QueryAsync<ProductReport>(sql, new { StartDate = filter.StartDate, EndDate = filter.EndDate, CategoryId = filter.CategoryId });
    }

    public async Task<IEnumerable<ProductReport>> GetAllBySupplierAsync(ProductReportFilter filter)
    {
        var sql = "SELECT * FROM ViewProductReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate AND SupplierId = @SupplierId";
        return await _dapper.QueryAsync<ProductReport>(sql, new { StartDate = filter.StartDate, EndDate = filter.EndDate, SupplierId = filter.SupplierId });
    }
}