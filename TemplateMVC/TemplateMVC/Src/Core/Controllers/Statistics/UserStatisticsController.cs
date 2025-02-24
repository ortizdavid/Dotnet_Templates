using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Statistics;

namespace TemplateMVC.Core.Controllers.Statistics;

[Route("statistics/users")]
public class UserStatisticsController : Controller
{
    private readonly UserStatisticsService _service;
    
    public UserStatisticsController(UserStatisticsService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(StatisticsViewPath.Get("Users", "Index"));
    }

    [HttpGet("actives-and-inactives")]
    public async Task<IActionResult> TotalActiveAndInactive()
    {
        var statistics = await _service.GetUserActivesAndInactives();
        return View(StatisticsViewPath.Get("Users", "TotalActivesAndInactives"), statistics);
    }
    
    [HttpGet("percentage-actives-and-inactives")]
    public async Task<IActionResult> PercentageOfActiveAndInactive()
    {
        var statistics = await _service.GetUserPercentageActivesAndInactives();
        return View(StatisticsViewPath.Get("Users", "PercentageActivesAndInactives"), statistics);
    }
}