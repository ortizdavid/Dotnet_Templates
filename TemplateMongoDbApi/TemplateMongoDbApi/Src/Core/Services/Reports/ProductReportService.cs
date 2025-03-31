using TemplateApi.Common.Exceptions;
using TemplateApi.Core.Models.Reports;
using TemplateApi.Core.Repositories.Reports;

namespace TemplateApi.Core.Services.Reports;

public class ProductReportService
{
    private readonly ProductReportRepository _repository;

    public ProductReportService(ProductReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReportResponse<ProductReport>> GetAllProducts(ReportFilter filter)
    {
        if (filter is null)
        {
            throw new BadRequestException("The report filter is required and must include valid dates and format.");
        }
        var products = await _repository.GetAllAsync(filter);
        return new ReportResponse<ProductReport>
        {
            Items = products,
            Metadata = new Metadata
            {
                TotalRecords = products.Count(),
                StartDate = filter.StartDate.ToString("yyyy-MM-dd"), 
                EndDate = filter.EndDate.ToString("yyyy-MM-dd")     
            }
        };       
    }
}