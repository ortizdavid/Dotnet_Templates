using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Models.Reports;
using System.Net;
using TemplateApi.Core.Services.Reports;
using TemplateApi.Common.Exceptions;
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
        try
        {
            var products = await _service.GetAllProducts(filter);
            return ReportFormat.Handle(_generator, products, filter.Format, "All_Products");
        }
        catch (AppException ex) 
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = $"An error occurred: {ex.Message}" });
        }
    }
}
