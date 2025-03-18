using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateEventDriven.Core.Models.Reports;
using TemplateEventDriven.Core.Models.Suppliers;
using TemplateEventDriven.Core.Services.Reports;

namespace TemplateEventDriven.Core.Controllers.Reports;

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