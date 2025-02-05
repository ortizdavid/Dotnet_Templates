using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Models.Suppliers;
using TemplateMVC.Core.Services.Reports;

namespace TemplateMVC.Core.Controllers.Reports;

[Authorize]
[Route("api/suppliers-report")]

public class SupplierReportController : Controller
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
        try
        {
            var suppliers = await _service.GetAllSuppliers(filter);
            return ReportFormat.Handle(_generator, suppliers, filter.Format, "All_Suppliers");
        }
        catch (AppException ex) 
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = $"An error occurred: {ex.Message}" });
        }
    }
}