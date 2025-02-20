using TemplateMVC.Core.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Auth;
using TemplateMVC.Common.Exceptions;

namespace TemplateMVC.Core.Controllers.Auth;

[Route("auth")]
public class AuthController : Controller
{
    private readonly AuthService _service;
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(AuthService service, IUserService userService, ILogger<AuthController> logger)
    {
        _service = service;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.Authenticate(viewModel);
            _logger.LogInformation($"User '{viewModel.UserName}' logged in sucessully");
            return Redirect("/home/index");
        }
         catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [Route("logout")]
    public IActionResult Logout()
    {
        try
        {
            _service.Logout();
            _logger.LogInformation("User Logged out sucessfully!");
            return RedirectToAction(nameof(Logout));
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View("Login");
        }
    }

    [HttpGet("get-recover-link")]
    public IActionResult GetRecoverLink()
    {
        return View();
    }

    [HttpPost("get-recover-link")]
    public async Task<IActionResult> GetRecoverLink(GetRecoverLinkViewModel viewModel)
    {
        try
        {
            await _service.GetRecoverLink(viewModel);
            var msg = $"Recovery link sent to email '{viewModel.Email}'";
            _logger.LogInformation(msg);
            return Redirect(nameof(Login));
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("recover-password/{token}")]
    public async Task<IActionResult> RecoverPassword(string token)
    {
        try
        {
            var user = await _userService.GetUserByRecoveryToken(token);
            ViewBag.User = user;
            return View();
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpPost("recover-password/{token}")]
    public async Task<IActionResult> RecoverPassword(ChangePasswordViewModel viewModel, string token)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.RecoverPassword(viewModel, token);
            var msg = $"Password recovered successfully";
            ViewBag.Message = msg;
            _logger.LogInformation(msg);
            return Redirect("/auth/login");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }
}
