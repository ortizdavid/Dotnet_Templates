using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Models.Reports;
using TemplateApi.Core.Services.Reports;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Reports;

[Authorize]
[Route("api/products-report")]
[ApiController]
public class ProductReportController : ControllerBase
{
    private readonly ProductReportService _service;
    private readonly IGenerator<ProductReport> _generator;

    public ProductReportController(ProductReportService service)
    {
        _service = service;
        _generator = new ProductGenerator();
    }

    [HttpGet("all-products")]
    public async Task<IActionResult> GeAllProducts([FromQuery] ReportFilter filter)
    {
        var products = await _service.GetAllProducts(filter);
        return ReportFormat.Handle(_generator, products, filter.Format, "All_Products");
    }
}
