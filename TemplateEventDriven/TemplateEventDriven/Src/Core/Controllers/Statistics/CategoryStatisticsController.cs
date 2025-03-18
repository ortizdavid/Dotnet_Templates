using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateEventDriven.Core.Services.Statistics;

namespace TemplateEventDriven.Core.Controllers.Statistics;

[Authorize]
[Route("api/category-statistics")]
public class CategoryStatisticsController : ControllerBase
{
    private readonly CategoryStatisticsService _service;
    private readonly ILogger<CategoryStatisticsController> _logger;

    public CategoryStatisticsController(CategoryStatisticsService service, ILogger<CategoryStatisticsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("top-categories")]
    public async Task<IActionResult> GetTopCategories()
    {
        var statistics = await _service.GetCategoryTopCategories();
        return Ok(statistics);
    }
}