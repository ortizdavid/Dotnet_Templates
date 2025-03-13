using Microsoft.AspNetCore.Mvc;
using TemplateEventDriven.Core.Services.Suppliers;
using TemplateEventDriven.Common.Helpers;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateEventDriven.Core.Controllers.Suppliers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly SupplierCommandService _commandService;
    private readonly SupplierQueryService _queryService;
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(SupplierCommandService commandService, SupplierQueryService queryService, ILogger<SuppliersController> logger)
    {
        _commandService = commandService;
        _queryService = queryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers([FromQuery]PaginationParam param)
    {
        var suppliers = await _queryService.GetAllSuppliers(param);
        return Ok(suppliers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody]SupplierRequest request)
    {
        await _commandService.CreateSupplier(request);
        var msg = $"Supplier '{request.SupplierName}' created.";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetSupplier(Guid uniqueId)
    {
        var supplier = await _queryService.GetSupplierByUniqueId(uniqueId);
        return Ok(supplier);
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateSupplier([FromBody]SupplierRequest request, Guid uniqueId)
    {
        await _commandService.UpdateSupplier(request, uniqueId);
        var msg = $"Supplier '{request.SupplierName}' updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }
    
    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteSupplier(Guid uniqueId)
    {
        await _commandService.DeleteSupplier(uniqueId);
        _logger.LogInformation($"Supplier with ID '{uniqueId}' deleted.");
        return NoContent();
    }

    [HttpGet("{uniqueId}/products")]
    public async Task<IActionResult> GetSupplierProducts(Guid uniqueId)
    {
        var products = await _queryService.GetSupplierProducts(uniqueId);
        return Ok(products);
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportSuppiersCSV(IFormFile file)
    {
        await _commandService.ImportSuppliersCSV(file);
        var msg = $"Suppliers imported by CSV successfully";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }
}
