using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Products;
using TemplateApi.Core.Models.Products;
using Microsoft.AspNetCore.Authorization;
using TemplateApi.Helpers;
using TemplateApi.Common.Exceptions;
using System.Net;

namespace TemplateApi.Core.Controllers.Products;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;
    private readonly ILogger<CategoriesController> _logger;
    
    public CategoriesController(CategoryService service, ILogger<CategoriesController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery]PaginationParam param)
    {
        try
        {
            var categories = await _service.GetAllCategories(param);
            return Ok(categories);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });;
        }
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetCategoryByUniqueId(Guid uniqueId)
    {
        try
        {
            var category = await _service.GetCategoryByUniqueId(uniqueId);
            return Ok(category);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
    {
        try
        {
            await _service.CreateCategory(request);
            var msg = $"Category '{request.CategoryName}' was created.";
            _logger.LogInformation(msg);
            return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });;
        }
    }


    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request, Guid uniqueId)
    {
        try
        {
            await _service.UpdateCategory(request, uniqueId);
            var msg = $"Category '{request.CategoryName}' was updated.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });;
        }
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteCategory(Guid uniqueId)
    {
        try
        {
            await _service.DeleteCategory(uniqueId);
            _logger.LogInformation($"Category deleted.");
            return NoContent();
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });;
        }
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCategories(IFormFile file)
    {
        try
        {
            await _service.ImportCategoriesCSV(file);
            var msg = $"Categories imported by CSV successfully";
            _logger.LogInformation(msg);
            return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
        }
    }
}

