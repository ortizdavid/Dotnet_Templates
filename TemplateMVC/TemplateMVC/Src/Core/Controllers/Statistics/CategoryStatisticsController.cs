using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Route("statistics/categories")]
public class CategoryStatisticsController : Controller
{
    private readonly CategoryStatisticsService _service;

    public CategoryStatisticsController(CategoryStatisticsService service)
    {
        _service = service;
    }
    
    public IActionResult Index()
    {
        return View(StatisticsViewPath.Get("Categories", "Index"));
    }

    [HttpGet("top-categories")]
    public async Task<IActionResult> GetTopCategories()
    {
        var statistics = await _service.GetCategoryTopCategories();
        return View(StatisticsViewPath.Get("Categories", "TopCategories"), statistics);
    }
}