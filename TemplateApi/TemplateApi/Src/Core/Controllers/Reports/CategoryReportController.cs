using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Reports;
using TemplateApi.Core.Models.Reports;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Reports;

[Authorize]
[Route("api/categories-report")]
[ApiController]
public class CategoryReportController : ControllerBase
{
    private readonly CategoryReportService _service;
    private readonly IGenerator<CategoryReport> _generator;

    public CategoryReportController(CategoryReportService service)
    {
        _service = service;
        _generator = new CategoryGenerator();
    }
    
    [HttpGet("all-categories")]
    public async Task<IActionResult> GeAllCategories([FromQuery] ReportFilter filter)
    {
        var categories = await _service.GetAllCategories(filter);
        return ReportFormat.Handle(_generator, categories, filter.Format, "All_Categories");
    }
}
