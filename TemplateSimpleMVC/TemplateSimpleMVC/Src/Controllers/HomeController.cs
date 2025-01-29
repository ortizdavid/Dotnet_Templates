using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TemplateSimpleMVC.Models;
using TemplateSimpleMVC.Repositories;

namespace TemplateSimpleMVC.Controllers;

public class HomeController : Controller
{
    private readonly UserRepository _repository;
    private readonly ILogger<HomeController> _logger;
    private readonly UserContext _userContext;

    public HomeController(UserRepository repository, UserContext userContext, ILogger<HomeController> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.Title = "Home";
        ViewBag.LoggedUser = _userContext.GetLoggedUser();
        return View();
    }
}
