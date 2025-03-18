using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Route("statistics/products")]
public class ProductStatisticsController : Controller
{
    private readonly ProductStatisticsService _service;

    public ProductStatisticsController(ProductStatisticsService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(StatisticsViewPath.Get("Products", "Index"));
    }

    [HttpGet("total-price-by-categories")]
    public async Task<IActionResult> TotalPriceByCategories()
    {
        var statistics = await _service.GetProductTotalPriceByCategories();
        return View(StatisticsViewPath.Get("Products", "TotalPriceByCategories"), statistics);
    }

    [HttpGet("total-price-by-suppliers")]
    public async Task<IActionResult> TotalPriceBySuppliers()
    {
        var statistics = await _service.GetProductTotalPriceBySuppliers();
        return View(StatisticsViewPath.Get("Products", "TotalPriceBySuppliers"), statistics);;
    }
}