using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateEventDriven.Core.Services.Products;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Products;

namespace TemplateEventDriven.Core.Controllers.Products;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductCommandService _commandService;
    private readonly ProductQueryService _queryService;
    private readonly ILogger<ProductsController> _logger;
    
    public ProductsController(ProductCommandService commandService, ProductQueryService queryService, ILogger<ProductsController> logger)
    {
        _commandService = commandService;
        _queryService = queryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery]PaginationParam param)
    {
        var products = await _queryService.GetAllProducts(param);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductRequest request)
    {
        await _commandService.CreateProduct(request);
        var msg = $"Product '{request.ProductName}' created.";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateProduct([FromBody]ProductRequest request, Guid uniqueId)
    {
        await _commandService.UpdateProduct(request, uniqueId);
        var msg = $"Product '{uniqueId}' updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetProductByUniqueId(Guid uniqueId)
    {
        var product = await _queryService.GetProductByUniqueId(uniqueId);
        return Ok(product);
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteProduct(Guid uniqueId)
    {
        await _commandService.DeleteProduct(uniqueId);
        _logger.LogInformation($"Product '{uniqueId}' deleteted.");
        return NoContent();
    }

    [HttpPost("{uniqueId}/images")]
    public async Task<IActionResult> UploadProductImages(Guid uniqueId, IFormFileCollection files)
    {
        await _commandService.UploadProductImages(uniqueId, files);
        var msg = $"Product '{uniqueId}' images uploaded.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpGet("{uniqueId}/images")]
    public async Task<IActionResult> GetProductImages(Guid uniqueId)
    {
        var images = await _queryService.GetProductImages(uniqueId);
        return Ok(images);
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        await _commandService.ImportProductsCSV(file);
        var msg = $"Products imported by CSV successfully";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }
}
