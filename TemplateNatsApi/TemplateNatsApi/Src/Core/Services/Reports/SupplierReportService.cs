using TemplateNatsApi.Common.Exceptions;
using TemplateNatsApi.Core.Models.Reports;
using TemplateNatsApi.Core.Models.Suppliers;
using TemplateNatsApi.Core.Repositories.Reports;

namespace TemplateNatsApi.Core.Services.Reports;

public class SupplierReportService
{
    private readonly SupplierReportRepository _repository;

    public SupplierReportService(SupplierReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReportResponse<SupplierReport>> GetAllSuppliers(ReportFilter filter)
    {
        if (filter == null)
        {
            throw new BadRequestException("The report filter is required and must include valid dates and format.");
        }
        var suppliers = await _repository.GetAllAsync(filter);
        return new ReportResponse<SupplierReport>
        {
            Items = suppliers,
            Metadata = new Metadata
            {
                TotalRecords = suppliers.Count(),
                StartDate = filter.StartDate.ToString("yyyy-MM-dd"), 
                EndDate = filter.EndDate.ToString("yyyy-MM-dd")     
            }
        };       
    }
}