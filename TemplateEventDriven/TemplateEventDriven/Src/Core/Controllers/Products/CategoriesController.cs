using Microsoft.AspNetCore.Mvc;
using TemplateEventDriven.Core.Services.Products;
using TemplateEventDriven.Core.Models.Products;
using Microsoft.AspNetCore.Authorization;
using TemplateEventDriven.Common.Helpers;
using System.Net;
using TemplateEventDriven.Core.Services;

namespace TemplateEventDriven.Core.Controllers.Products;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly CategoryCommandService _commandService;
    private readonly CategoryQueryService _queryService;
    private readonly ILogger<CategoriesController> _logger;
    
    public CategoriesController(CategoryCommandService commandService, CategoryQueryService queryService, ILogger<CategoriesController> logger)
    {
        _commandService = commandService;
        _queryService = queryService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery]PaginationParam param)
    {
        var categories = await _queryService.GetAllCategories(param);
        return Ok(categories); 
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetCategoryByUniqueId(Guid uniqueId)
    {
        var category = await _queryService.GetCategoryByUniqueId(uniqueId);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
    {
        await _commandService.CreateCategory(request);
        var msg = $"Category '{request.CategoryName}' was created.";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }


    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request, Guid uniqueId)
    {
        await _commandService.UpdateCategory(request, uniqueId);
        var msg = $"Category '{request.CategoryName}' was updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteCategory(Guid uniqueId)
    {
        await _commandService.DeleteCategory(uniqueId);
        _logger.LogInformation($"Category deleted.");
        return NoContent();
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCategories(IFormFile file)
    {
        await _commandService.ImportCategoriesCSV(file);
        var msg = $"Categories imported by CSV successfully";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }
}
