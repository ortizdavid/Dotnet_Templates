using TemplateApi.Core.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Auth;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Auth;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(AuthService service, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request)
    {
        var tokenResponse = await _service.Authenticate(request);
        _logger.LogInformation($"User '{request.UserName}' logged in sucessully");
        return Ok(tokenResponse);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _service.Logout();
        var msg = "User Logged out sucessfully!";
        return Ok(new { Message = msg });
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var tokenResponse =  await _service.RefreshUserToken(request);
        _logger.LogInformation($"Token refreshed successfully");
        return Ok(tokenResponse);
    }

    [HttpPost("recover-password/{token}")]
    public async Task<IActionResult> RecoverPassword([FromBody]ChangePasswordRequest request, string token)
    {
        await _service.RecoverPassword(request, token);
        var msg = $"Password recovered successfully";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }
}
