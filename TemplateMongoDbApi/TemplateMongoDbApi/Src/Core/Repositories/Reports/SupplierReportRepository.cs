using System.Data;
using TemplateMongoDbApi.Core.Models.Reports;
using TemplateMongoDbApi.Core.Models.Suppliers;

namespace TemplateMongoDbApi.Core.Repositories.Reports;

public class SupplierReportRepository
{
    public SupplierReportRepository()
    {
    }

    public async Task<IEnumerable<SupplierReport>> GetAllAsync(ReportFilter filter)
    {
        var sql = "SELECT * FROM ViewSupplierReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
        return null;
    }
}