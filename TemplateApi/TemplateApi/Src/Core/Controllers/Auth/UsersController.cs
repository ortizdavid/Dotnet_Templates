using TemplateApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Core.Models.Auth;
using TemplateApi.Core.Services.Auth;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateApi.Core.Controllers.Auth;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _service;
    private readonly AuthService _authService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UserService service, AuthService authService, ILogger<UsersController> logger) 
    {
        _service = service;
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery]PaginationParam param)
    {
        var users = await _service.GetAllUsers(param);
        return Ok(users);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        await _service.CreateUser(request);
        var msg = $"User '{request.UserName}' was created";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetUserById(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
        return Ok(user);
    }

    [HttpGet("by-name/{userName}")]
    public async Task<IActionResult> GetUserByName(string userName)
    {
        var user = await _service.GetUserByName(userName);
        return Ok(user);
    }

    [HttpPut("{uniqueId}/upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file, Guid uniqueId)
    {
        await _service.UploadUserImage(file, uniqueId);
        var msg = $"User '{uniqueId}' image uploaded.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpPut("{uniqueId}/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request, Guid uniqueId)
    {
        await _service.ChangePassword(request, uniqueId);
        var msg = $"User '{uniqueId}' password was changed";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteUser(Guid uniqueId)
    {
        await _service.DeleteUser(uniqueId);
        _logger.LogInformation($"User with ID '{uniqueId}' was deleted");
        return NoContent();
    }

    [HttpPut("{uniqueId}/activate")]
    public async Task<IActionResult> ActivateUser(Guid uniqueId)
    {
        await _service.ActivateUser(uniqueId);
        var msg = $"User with ID '{uniqueId}' was activated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg }); 
    }

    [Authorize]
    [HttpPut("{uniqueId}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid uniqueId)
    {
        await _service.DeactivateUser(uniqueId);
        var msg = $"User with ID '{uniqueId}' was deactivated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });  
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _authService.GetLoggedUser();
        return Ok(user);
    }
}
