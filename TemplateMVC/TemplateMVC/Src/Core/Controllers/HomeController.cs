using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Auth;

namespace TemplateMVC.Core.Controllers;

[Route("home")]
public class HomeController : Controller
{
    private readonly AuthService _authService;
    public HomeController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        var loggedUser = await _authService.GetLoggedUser();
        ViewBag.LoggedUser = loggedUser;
        return View();
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> CurrentUser()
    {
        var loggedUser = await _authService.GetLoggedUser();
        ViewBag.LoggedUser = loggedUser;
        return View();
    }
}