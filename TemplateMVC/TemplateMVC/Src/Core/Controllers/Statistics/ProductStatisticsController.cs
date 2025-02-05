using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Authorize]
[Route("api/product-statistics")]
public class ProductStatisticsController : Controller
{
    private readonly ProductStatisticsService _service;

    public ProductStatisticsController(ProductStatisticsService service)
    {
        _service = service;
    }

    [HttpGet("total-price-by-categories")]
    public async Task<IActionResult> TotalPriceByCategories()
    {
        var statistics = await _service.GetProductTotalPriceByCategories();
        return Ok(statistics);
    }

    [HttpGet("total-price-by-suppliers")]
    public async Task<IActionResult> TotalPriceBySuppliers()
    {
        var statistics = await _service.GetProductTotalPriceBySuppliers();
        return Ok(statistics);
    }
}