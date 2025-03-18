using TemplateMVC.Common.Exceptions;
using TemplateMVC.Common.Helpers;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Models.Suppliers;
using TemplateMVC.Core.Repositories.Reports;

namespace TemplateMVC.Core.Services.Reports;

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
        if (!filter.HasValidDateRange)
        {
            throw new BadRequestException("StartDate must be earlier than or equal to EndDate.");
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