using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Reports;
using TemplateApi.Core.Models.Reports;
using System.Net;
using TemplateApi.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Reports
{
    [Authorize]
    [Route("api/categories-report")]
    [ApiController]
    public class CategoryReportController : ControllerBase
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
}
