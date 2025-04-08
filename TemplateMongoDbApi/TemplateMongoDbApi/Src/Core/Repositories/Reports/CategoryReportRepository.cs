using TemplateMongoDbApi.Core.Models.Reports;

namespace TemplateMongoDbApi.Core.Repositories.Reports;

public class CategoryReportRepository
{
    public CategoryReportRepository()
    {
    }

    public async Task<IEnumerable<CategoryReport>> GetAllAsync(ReportFilter filter)
    {
        var sql = "SELECT * FROM ViewCategoryReportData WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
        return null; 
    }
}