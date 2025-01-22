using TemplateApi.Core.Models.Auth;
using TemplateApi.Core.Repositories.Auth;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Services.Auth;
using TemplateApi.Common.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Auth
{
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
            try
            {
                var tokenResponse = await _service.Authenticate(request);
                _logger.LogInformation($"User '{request.UserName}' logged in sucessully");
                return Ok(tokenResponse);
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _service.Logout();
                var msg = "User Logged out sucessfully!";
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

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var tokenResponse =  await _service.RefreshUserToken(request);
                _logger.LogInformation($"Token refreshed successfully");
                return Ok(tokenResponse);
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

        [HttpPost("recover-password/{token}")]
        public async Task<IActionResult> RecoverPassword([FromBody]ChangePasswordRequest request, string token)
        {
            try
            {
                await _service.RecoverPassword(request, token);
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
}
