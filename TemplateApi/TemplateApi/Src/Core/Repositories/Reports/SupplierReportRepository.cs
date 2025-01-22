using System.Data;
using Dapper;
using TemplateApi.Core.Models.Reports;
using TemplateApi.Core.Models.Suppliers;

namespace TemplateApi.Core.Repositories.Reports
{
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
}