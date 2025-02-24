using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Models.Suppliers;
using TemplateMVC.Core.Services.Reports;

namespace TemplateMVC.Core.Controllers.Reports;

[Route("reports/suppliers")]
public class SuppliersReportController : Controller
{
    private readonly SupplierReportService _service;
    private readonly IGenerator<SupplierReport> _generator;
    private readonly ILogger<SuppliersReportController> _logger;

    public SuppliersReportController(SupplierReportService service, ILogger<SuppliersReportController> logger)
    {
        _service = service;
        _generator = new SupplierGenerator();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(ReportViewPath.Get("Suppliers", "AllSuppliers"));
    }

    [HttpGet("all-suppliers")]
    public IActionResult AllSuppliers()
    {
        return View(ReportViewPath.Get("Suppliers", "AllSuppliers"));
    }

    [HttpPost("all-suppliers")]
    public async Task<IActionResult> AllSuppliers(ReportFilter filter)
    {
        try
        {
            var suppliers = await _service.GetAllSuppliers(filter);
            return ReportFormat.Handle(_generator, suppliers, filter.Format, "All_Suppliers");
        }
        catch (AppException ex) 
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}