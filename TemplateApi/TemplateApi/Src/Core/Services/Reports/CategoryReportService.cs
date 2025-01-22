using TemplateApi.Common.Exceptions;
using TemplateApi.Core.Models.Reports;
using TemplateApi.Core.Repositories.Reports;

namespace TemplateApi.Core.Services.Reports
{
    public class CategoryReportService
    {
        private readonly CategoryReportRepository _repository;

        public CategoryReportService(CategoryReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReportResponse<CategoryReport>> GetAllCategories(ReportFilter filter)
        {
            if (filter is null)
            {
                throw new BadRequestException("The report filter is required and must include valid dates and format.");
            }
            var categories = await _repository.GetAllAsync(filter);
            return new ReportResponse<CategoryReport>
            {
                Items = categories,
                Metadata = new Metadata
                {
                    TotalRecords = categories.Count(),
                    StartDate = filter.StartDate.ToString("yyyy-MM-dd"), 
                    EndDate = filter.EndDate.ToString("yyyy-MM-dd")     
                }
            };            
        }
    }
}