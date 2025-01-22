using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Suppliers;
using TemplateApi.Core.Models.Suppliers;
using TemplateApi.Helpers;
using TemplateApi.Common.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Suppliers
{
    [Authorize]
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
            try
            {
                var suppliers = await _service.GetAllSuppliers(param);
                return Ok(suppliers);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody]SupplierRequest request)
        {
            try
            {
                await _service.CreateSupplier(request);
                var msg = $"Supplier '{request.SupplierName}' created.";
                _logger.LogInformation(msg);
                return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpGet("{uniqueId}")]
        public async Task<IActionResult> GetSupplier(Guid uniqueId)
        {
            try 
            {
                var supplier = await _service.GetSupplierByUniqueId(uniqueId);
                return Ok(supplier);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpPut("{uniqueId}")]
        public async Task<IActionResult> UpdateSupplier([FromBody]SupplierRequest request, Guid uniqueId)
        {
            try
            {
                await _service.UpdateSupplier(request, uniqueId);
                var msg = $"Supplier '{request.SupplierName}' updated.";
                _logger.LogInformation(msg);
                return Ok(new { Message = msg });
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }
        
        [HttpDelete("{uniqueId}")]
        public async Task<IActionResult> DeleteSupplier(Guid uniqueId)
        {
            try
            {
                await _service.DeleteSupplier(uniqueId);
                _logger.LogInformation($"Supplier with ID '{uniqueId}' deleted.");
                return NoContent();
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpGet("{uniqueId}/products")]
        public async Task<IActionResult> GetSupplierProducts(Guid uniqueId)
        {
            try
            {
                var products = await _service.GetSupplierProducts(uniqueId);
                return Ok(products);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpPost("import-csv")]
        public async Task<IActionResult> ImportSuppiersCSV(IFormFile file)
        {
            try
            {
                await _service.ImportSuppliersCSV(file);
                var msg = $"Suppliers imported by CSV successfully";
                _logger.LogInformation(msg);
                return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }
    }
}
