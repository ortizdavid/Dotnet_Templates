using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMongoDbApi.Common.Helpers;
using TemplateMongoDbApi.Core.DTOs.Products;
using TemplateMongoDbApi.Core.Services.Products;

namespace TemplateMongoDbApi.Core.Controllers.Products;

//[Authorize]
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

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct([FromBody]ProductRequest request, string productId)
    {
        await _service.UpdateProduct(request, productId);
        var msg = $"Product '{productId}' updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductByUniqueId(string productId)
    {
        var product = await _service.GetProductByUniqueId(productId);
        return Ok(product);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(string productId)
    {
        await _service.DeleteProduct(productId);
        _logger.LogInformation($"Product '{productId}' deleteted.");
        return NoContent();
    }

    [HttpPost("{productId}/images")]
    public async Task<IActionResult> UploadProductImages(string productId, IFormFileCollection files)
    {
        await _service.UploadProductImages(productId, files);
        var msg = $"Product '{productId}' images uploaded.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpGet("{productId}/images")]
    public async Task<IActionResult> GetProductImages(string productId)
    {
        var images = await _service.GetProductImages(productId);
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
