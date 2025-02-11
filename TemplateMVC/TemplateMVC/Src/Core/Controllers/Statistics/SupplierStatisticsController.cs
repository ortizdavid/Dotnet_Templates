using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Authorize]
[Route("api/supplier-statistics")]    
public class SupplierStatisticsController : Controller
{
    private readonly SupplierStatisticsService _service;

    public SupplierStatisticsController(SupplierStatisticsService service)
    {
        _service = service;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("top-suppliers")]
    public async Task<IActionResult> GetTopSuppliers()
    {
        var statistics = await _service.GetTopSuppliers();
        return Ok(statistics);
    }
}