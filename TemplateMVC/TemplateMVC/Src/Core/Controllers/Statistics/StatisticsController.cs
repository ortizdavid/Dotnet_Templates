using Microsoft.AspNetCore.Mvc;

namespace TemplateMVC.Core.Controllers.Statistics;

[Route("statistics")]
public class StatisticsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
