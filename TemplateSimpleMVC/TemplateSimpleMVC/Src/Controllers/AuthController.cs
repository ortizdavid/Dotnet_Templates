using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TemplateSimpleMVC.Helpers;
using TemplateSimpleMVC.Models;
using TemplateSimpleMVC.Repositories;

namespace TemplateSimpleMVC.Controllers;

[Route("Auth")]
public class AuthController : Controller
{
    private readonly UserRepository _repository;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserRepository repository, ILogger<AuthController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]
    public IActionResult Login(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = _repository.FindByName(model.UserName);
        var userName = user?.UserName ?? string.Empty;
        if (user is null || !PasswordHelper.Verify(model.Password, user.Password))
        {
            var msg = "Invalid Username or Password";
            _logger.LogError(msg);
            ModelState.AddModelError("", msg);
            return View(model);
        }
        HttpContext.Session.SetString("UserName", userName);
        HttpContext.Session.SetInt32("UserId", user.Id);
        _logger.LogInformation($"User '{userName}' logged successfully!");
        return Redirect("/Home/Index");
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        ViewBag.Title = "Logout";
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }
}
