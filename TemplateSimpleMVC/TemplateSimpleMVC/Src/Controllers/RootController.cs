using System.Net;
using Microsoft.AspNetCore.Mvc;
using TemplateSimpleMVC.Helpers;
using TemplateSimpleMVC.Models;
using TemplateSimpleMVC.Repositories;

namespace TemplateSimpleMVC.Controllers;

[Route("")]
public class RootController : Controller
{
    private readonly UserRepository _repository;
    private readonly ILogger<RootController> _logger;

    public RootController(UserRepository repository, ILogger<RootController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet("Register")]
    [ValidateAntiForgeryToken]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("Register")]
    public IActionResult Register(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userName = model.UserName ?? string.Empty;
        if (_repository.Exists("UserName", userName))
        {
            var msg = $"User '{userName}' already exists";
            _logger.LogError(msg);
            ModelState.AddModelError("", msg);
            return View(model);
        }
        var user = new User()
        {
            UserName = model.UserName,
            Password = PasswordHelper.Hash(model.Password),
        };
        _repository.Create(user);
        _logger.LogInformation($"User '{userName}' created successfully");
        return RedirectToAction("Login", "Auth");
    }
}