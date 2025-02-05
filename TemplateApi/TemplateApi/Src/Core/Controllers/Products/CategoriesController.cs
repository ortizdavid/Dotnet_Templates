using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Products;
using TemplateApi.Core.Models.Products;
using Microsoft.AspNetCore.Authorization;
using TemplateApi.Helpers;
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
        var categories = await _service.GetAllCategories(param);
        return Ok(categories); 
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetCategoryByUniqueId(Guid uniqueId)
    {
        var category = await _service.GetCategoryByUniqueId(uniqueId);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
    {
        await _service.CreateCategory(request);
        var msg = $"Category '{request.CategoryName}' was created.";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }


    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request, Guid uniqueId)
    {
        await _service.UpdateCategory(request, uniqueId);
        var msg = $"Category '{request.CategoryName}' was updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteCategory(Guid uniqueId)
    {
        await _service.DeleteCategory(uniqueId);
        _logger.LogInformation($"Category deleted.");
        return NoContent();
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCategories(IFormFile file)
    {
        await _service.ImportCategoriesCSV(file);
        var msg = $"Categories imported by CSV successfully";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }
}
