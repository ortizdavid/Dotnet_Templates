using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Helpers;
using TemplateApi.Core.Models.Products;
using TemplateApi.Core.Services.Products;

namespace TemplateApi.Core.Controllers.Products;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;
    private readonly ILogger<ProductsController> _logger;
    
    public ProductsController(ProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery]PaginationParam param)
    {
        var products = await _service.GetAllProducts(param);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductRequest request)
    {
        await _service.CreateProduct(request);
        var msg = $"Product '{request.ProductName}' created.";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateProduct([FromBody]ProductRequest request, Guid uniqueId)
    {
        await _service.UpdateProduct(request, uniqueId);
        var msg = $"Product '{uniqueId}' updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetProductByUniqueId(Guid uniqueId)
    {
        var product = await _service.GetProductByUniqueId(uniqueId);
        return Ok(product);
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteProduct(Guid uniqueId)
    {
        await _service.DeleteProduct(uniqueId);
        _logger.LogInformation($"Product '{uniqueId}' deleteted.");
        return NoContent();
    }

    [HttpPost("{uniqueId}/images")]
    public async Task<IActionResult> UploadProductImages(Guid uniqueId, IFormFileCollection files)
    {
        await _service.UploadProductImages(uniqueId, files);
        var msg = $"Product '{uniqueId}' images uploaded.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpGet("{uniqueId}/images")]
    public async Task<IActionResult> GetProductImages(Guid uniqueId)
    {
        var images = await _service.GetProductImages(uniqueId);
        return Ok(images);
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        await _service.ImportProductsCSV(file);
        var msg = $"Products imported by CSV successfully";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }
}
