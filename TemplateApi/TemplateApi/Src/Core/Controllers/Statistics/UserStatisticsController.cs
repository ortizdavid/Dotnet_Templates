using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Statistics;

namespace TemplateApi.Core.Controllers.Statistics
{
    [Authorize]
    [Route("api/user-statistics")]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private readonly UserStatisticsService _service;
        private readonly ILogger<UserStatisticsController> _logger;
        
        public UserStatisticsController(UserStatisticsService service, ILogger<UserStatisticsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("active-inactive")]
        public async Task<IActionResult> TotalActiveAndInactive()
        {
            var statistics = await _service.GetUserActivesAndInactives();
            return Ok(statistics);
        }

        
        [HttpGet("percent-active-inactive")]
        public async Task<IActionResult> PercentageOfActiveAndInactive()
        {
            var statistics = await _service.GetUserPercentageActivesAndInactives();
            return Ok(statistics);
        }
    }
}