using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Authorize]
[Route("api/category-statistics")]
public class CategoryStatisticsController : Controller
{
    private readonly CategoryStatisticsService _service;

    public CategoryStatisticsController(CategoryStatisticsService service)
    {
        _service = service;
    }

    [HttpGet("top-categories")]
    public async Task<IActionResult> GetTopCategories()
    {
        var statistics = await _service.GetCategoryTopCategories();
        return Ok(statistics);
    }
}