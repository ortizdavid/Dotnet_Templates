using Microsoft.AspNetCore.Mvc;
using TemplateMongoDbApi.Core.Services.Products;
using TemplateMongoDbApi.Core.Models.Products;
using Microsoft.AspNetCore.Authorization;
using TemplateMongoDbApi.Common.Helpers;
using System.Net;
using MongoDB.Bson;

namespace TemplateMongoDbApi.Core.Controllers.Products;

//[Authorize]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryByUniqueId(ObjectId id)
    {
        var category = await _service.GetCategoryByUniqueId(id);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request, ObjectId id)
    {
        await _service.UpdateCategory(request, id);
        var msg = $"Category '{request.CategoryName}' was updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(ObjectId id)
    {
        await _service.DeleteCategory(id);
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
