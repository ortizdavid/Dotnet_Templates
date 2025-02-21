using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Reports;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Common.Exceptions;

namespace TemplateMVC.Core.Controllers.Reports;

[Route("reports/categories")]
public class CategoriesReportController : Controller
{
    private readonly CategoryReportService _service;
    private readonly IGenerator<CategoryReport> _generator;
    private readonly ILogger<CategoriesReportController> _logger;

    public CategoriesReportController(CategoryReportService service, ILogger<CategoriesReportController> logger)
    {
        _service = service;
        _generator = new CategoryGenerator();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Reports/Categories/Index.cshtml");
    }

    [HttpGet("all-categories")]
    public IActionResult AllCategories()
    {
        return View("~/Views/Reports/Categories/AllCategories.cshtml");
    }

    [HttpPost("all-categories")]
    public async Task<IActionResult> AllCategories(ReportFilter filter)
    {
        try
        {
            var categories = await _service.GetAllCategories(filter);
            ViewBag.ResultsCount = categories.Items.Count(); 
            return ReportFormat.Handle(_generator, categories, filter.Format, "All_Categories");
        }
        catch (AppException ex) 
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View("~/Views/Reports/Categories/AllCategories.cshtml");
        }
    }
}
