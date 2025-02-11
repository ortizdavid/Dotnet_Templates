using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Models.Reports;
using System.Net;
using TemplateMVC.Core.Services.Reports;
using TemplateMVC.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace TemplateMVC.Core.Controllers.Reports;

[Authorize]
[Route("api/products-report")]

public class ProductReportController : Controller
{
    private readonly ProductReportService _service;
    private readonly IGenerator<ProductReport> _generator;

    public ProductReportController(ProductReportService service)
    {
        _service = service;
        _generator = new ProductGenerator();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("all-products")]
    public async Task<IActionResult> GeAllProducts([FromQuery] ReportFilter filter)
    {
        try
        {
            var products = await _service.GetAllProducts(filter);
            return ReportFormat.Handle(_generator, products, filter.Format, "All_Products");
        }
        catch (AppException ex) 
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }
}
