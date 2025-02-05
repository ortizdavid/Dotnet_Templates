using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Services.Products;

namespace TemplateMVC.Core.Controllers.Products;

public class ProductsController : Controller
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
        try
        {
            var products = await _service.GetAllProducts(param);
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductViewModel viewModel)
    {
        try
        {
            await _service.CreateProduct(viewModel);
            var msg = $"Product '{viewModel.ProductName}' created.";
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

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateProduct(ProductViewModel viewModel, Guid uniqueId)
    {
        try
        {
            await _service.UpdateProduct(viewModel, uniqueId);
            var msg = $"Product '{uniqueId}' updated.";
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

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetProductByUniqueId(Guid uniqueId)
    {
        try 
        {
            var product = await _service.GetProductByUniqueId(uniqueId);
            return Ok(product);
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
    public async Task<IActionResult> DeleteProduct(Guid uniqueId)
    {
        try
        {
            await _service.DeleteProduct(uniqueId);
            _logger.LogInformation($"Product '{uniqueId}' deleteted.");
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

    [HttpPost("{uniqueId}/images")]
    public async Task<IActionResult> UploadProductImages(Guid uniqueId, IFormFileCollection files)
    {
        try
        {
            await _service.UploadProductImages(uniqueId, files);
            var msg = $"Product '{uniqueId}' images uploaded.";
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

    [HttpGet("{uniqueId}/images")]
    public async Task<IActionResult> GetProductImages(Guid uniqueId)
    {
        try
        {
            var images = await _service.GetProductImages(uniqueId);
            return Ok(images);
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
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        try
        {
            await _service.ImportProductsCSV(file);
            var msg = $"Products imported by CSV successfully";
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

