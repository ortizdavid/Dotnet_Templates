using Microsoft.AspNetCore.Mvc;

namespace TemplateMVC.Core.Controllers.Reports;

[Route("reports")]
public class ReportsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}