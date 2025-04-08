using TemplateMongoDbApi.Core.Models.Reports;

namespace TemplateMongoDbApi.Core.Repositories.Reports;

public class ProductReportRepository
{
    public ProductReportRepository()
    {
    }

    public async Task<IEnumerable<ProductReport>> GetAllAsync(ReportFilter filter)
    {
        var sql = "SELECT * FROM ViewProductReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
        return null;
    }
}