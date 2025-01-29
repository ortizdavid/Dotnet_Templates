using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Statistics;

namespace TemplateApi.Core.Controllers.Statistics;

[Authorize]
[Route("api/product-statistics")]
public class ProductStatisticsController : ControllerBase
{
    private readonly ProductStatisticsService _service;
    private readonly ILogger<ProductStatisticsController> _logger;

    public ProductStatisticsController(ProductStatisticsService service, ILogger<ProductStatisticsController> logger)
    {
        _service = service;
        _logger = logger;
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