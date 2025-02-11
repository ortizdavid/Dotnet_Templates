using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Authorize]
[Route("api/user-statistics")]

public class UserStatisticsController : Controller
{
    private readonly UserStatisticsService _service;
    
    public UserStatisticsController(UserStatisticsService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
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