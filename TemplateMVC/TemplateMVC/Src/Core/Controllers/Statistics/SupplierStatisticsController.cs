using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Route("statistics/suppliers")]    
public class SupplierStatisticsController : Controller
{
    private readonly SupplierStatisticsService _service;

    public SupplierStatisticsController(SupplierStatisticsService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(StatisticsViewPath.Get("Suppliers", "Index"));
    }

    [HttpGet("top-suppliers")]
    public async Task<IActionResult> GetTopSuppliers()
    {
        var statistics = await _service.GetTopSuppliers();
        return View(StatisticsViewPath.Get("Suppliers", "TopSuppliers"), statistics);
    }
}