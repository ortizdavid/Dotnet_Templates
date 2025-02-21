using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Services.Auth;

namespace TemplateMVC.Core.Controllers;

[Route("home")]
public class HomeController : Controller
{
    private readonly AuthService _authService;
    private readonly IUserService _userService;
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(AuthService authService, IUserService userService, ILogger<HomeController> logger, IConfiguration configuration)
    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
        _configuration = configuration;
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
        ViewBag.UserImage = $"{_configuration["UploadsDirectory"]}/Users/{loggedUser?.Image}";
        return View();
    }

    [HttpGet("upload-image")]
    public IActionResult UploadImage()
    {
        return View();
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();   
            }
            var loggedUser = await _authService.GetLoggedUser();
            ViewBag.LoggedUser = loggedUser;
            await _userService.UploadUserImage(file, loggedUser.UniqueId);
            return Redirect("/home/current-user");
        }
         catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("change-password")]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();   
            }
            var loggedUser = await _authService.GetLoggedUser();
            ViewBag.LoggedUser = loggedUser;
            await _userService.ChangePassword(viewModel, loggedUser.UniqueId);
            return Redirect("/home/current-user");
        }
         catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}