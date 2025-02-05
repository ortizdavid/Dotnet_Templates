using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Reports;
using TemplateMVC.Core.Models.Reports;
using System.Net;
using TemplateMVC.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace TemplateMVC.Core.Controllers.Reports;

[Authorize]
[Route("api/categories-report")]

public class CategoryReportController : Controller
{
    private readonly CategoryReportService _service;
    private readonly IGenerator<CategoryReport> _generator;

    public CategoryReportController(CategoryReportService service)
    {
        _service = service;
        _generator = new CategoryGenerator();
    }
    
    [HttpGet("all-categories")]
    public async Task<IActionResult> GeAllCategories([FromQuery] ReportFilter filter)
    {
        try
        {
            var categories = await _service.GetAllCategories(filter);
            return ReportFormat.Handle(_generator, categories, filter.Format, "All_Categories");
        }
        catch (AppException ex) 
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = $"An error occurred: {ex.Message}" });
        }
    }
}
