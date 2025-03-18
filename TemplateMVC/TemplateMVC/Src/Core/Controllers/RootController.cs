using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("")]
public class RootController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Login", "Auth");
    }
}
