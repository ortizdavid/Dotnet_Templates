using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Repositories.Auth;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Auth;
using TemplateMVC.Common.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateMVC.Core.Controllers.Auth;

public class AuthController : Controller
{
    private readonly AuthService _service;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(AuthService service, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
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
            return Redirect("/Home/Index");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return View();
        }
    }

    public IActionResult Logout()
    {
        try
        {
            _service.Logout();
            _logger.LogInformation("User Logged out sucessfully!");
            return RedirectToAction(nameof(Logout));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return View("Login");
        }
    }

    [AllowAnonymous]
    [HttpPost("recover-password/{token}")]
    public async Task<IActionResult> RecoverPassword(ChangePasswordViewModel viewModel, string token)
    {
        try
        {
            await _service.RecoverPassword(viewModel, token);
            var msg = $"Password recovered successfully";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = ex.Message });
        }
    }
}
