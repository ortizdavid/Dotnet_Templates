using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Statistics;

namespace TemplateApi.Core.Controllers.Statistics
{
    [Authorize]
    [Route("api/supplier-statistics")]    
    public class SupplierStatisticsController : ControllerBase
    {
        private readonly SupplierStatisticsService _service;
        private readonly ILogger<SupplierStatisticsController> _logger;

        public SupplierStatisticsController(SupplierStatisticsService service, ILogger<SupplierStatisticsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("top-suppliers")]
        public async Task<IActionResult> GetTopSuppliers()
        {
            var statistics = await _service.GetTopSuppliers();
            return Ok(statistics);
        }
    }
}