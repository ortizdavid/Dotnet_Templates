using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Models.Reports;
using TemplateApi.Core.Models.Suppliers;
using TemplateApi.Core.Services.Reports;

namespace TemplateApi.Core.Controllers.Reports;

[Authorize]
[Route("api/suppliers-report")]
[ApiController]
public class SupplierReportController : ControllerBase
{
    private readonly SupplierReportService _service;
    private readonly IGenerator<SupplierReport> _generator;

    public SupplierReportController(SupplierReportService service)
    {
        _service = service;
        _generator = new SupplierGenerator();
    }

    [HttpGet("all-suppliers")]
    public async Task<IActionResult> GetAllSuppliers([FromQuery]ReportFilter filter)
    {
        var suppliers = await _service.GetAllSuppliers(filter);
        return ReportFormat.Handle(_generator, suppliers, filter.Format, "All_Suppliers");
    }
}