using Microsoft.AspNetCore.Mvc;
using TemplateMongoDbApi.Core.Services.Suppliers;
using TemplateMongoDbApi.Common.Helpers;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateMongoDbApi.Core.Controllers.Suppliers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly SupplierService _service;
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(ILogger<SuppliersController> logger, SupplierService service)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers([FromQuery]PaginationParam param)
    {
        var suppliers = await _service.GetAllSuppliers(param);
        return Ok(suppliers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody]SupplierRequest request)
    {
        await _service.CreateSupplier(request);
        var msg = $"Supplier '{request.SupplierName}' created.";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }

    [HttpGet("{supplierId}")]
    public async Task<IActionResult> GetSupplier(string supplierId)
    {
        var supplier = await _service.GetSupplierById(supplierId);
        return Ok(supplier);
    }

    [HttpPut("{supplierId}")]
    public async Task<IActionResult> UpdateSupplier([FromBody]SupplierRequest request, string supplierId)
    {
        await _service.UpdateSupplier(request, supplierId);
        var msg = $"Supplier '{request.SupplierName}' updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }
    
    [HttpDelete("{supplierId}")]
    public async Task<IActionResult> DeleteSupplier(string supplierId)
    {
        await _service.DeleteSupplier(supplierId);
        _logger.LogInformation($"Supplier with ID '{supplierId}' deleted.");
        return NoContent();
    }

    [HttpGet("{supplierId}/products")]
    public async Task<IActionResult> GetSupplierProducts(string supplierId)
    {
        var products = await _service.GetSupplierProducts(supplierId);
        return Ok(products);
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportSuppiersCSV(IFormFile file)
    {
        await _service.ImportSuppliersCSV(file);
        var msg = $"Suppliers imported by CSV successfully";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }
}
