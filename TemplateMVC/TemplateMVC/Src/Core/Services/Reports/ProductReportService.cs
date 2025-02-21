using TemplateMVC.Common.Exceptions;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Repositories.Reports;

namespace TemplateMVC.Core.Services.Reports;

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
        if (!filter.HasValidDateRange)
        {
            throw new BadRequestException("StartDate must be earlier than or equal to EndDate.");
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

    public async Task<ReportResponse<ProductReport>> GetAllProductsByCategory(ProductReportFilter filter)
    {
        if (filter is null)
        {
            throw new BadRequestException("The report filter is required and must include valid dates and format.");
        }
        if (!filter.HasValidDateRange)
        {
            throw new BadRequestException("StartDate must be earlier than or equal to EndDate.");
        }
        var products = await _repository.GetAllByCategoryAsync(filter);
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

    public async Task<ReportResponse<ProductReport>> GetAllProductsBySupplier(ProductReportFilter filter)
    {
        if (filter is null)
        {
            throw new BadRequestException("The report filter is required and must include valid dates and format.");
        }
        if (!filter.HasValidDateRange)
        {
            throw new BadRequestException("StartDate must be earlier than or equal to EndDate.");
        }
        var products = await _repository.GetAllBySupplierAsync(filter);
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