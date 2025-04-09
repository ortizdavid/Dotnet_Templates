using TemplateMongoDbApi.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using TemplateMongoDbApi.Core.Services.Auth;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using TemplateMongoDbApi.Core.DTOs.Auth;

namespace TemplateMongoDbApi.Core.Controllers.Auth;

//[Authorize]
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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _service.GetUserById(userId);
        return Ok(user);
    }

    [HttpGet("by-name/{userName}")]
    public async Task<IActionResult> GetUserByName(string userName)
    {
        var user = await _service.GetUserByName(userName);
        return Ok(user);
    }

    [HttpPut("{userId}/upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file, string userId)
    {
        await _service.UploadUserImage(file, userId);
        var msg = $"User '{userId}' image uploaded.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpPut("{userId}/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request, string userId)
    {
        await _service.ChangePassword(request, userId);
        var msg = $"User '{userId}' password was changed";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _service.DeleteUser(userId);
        _logger.LogInformation($"User with ID '{userId}' was deleted");
        return NoContent();
    }

    [HttpPut("{userId}/activate")]
    public async Task<IActionResult> ActivateUser(string userId)
    {
        await _service.ActivateUser(userId);
        var msg = $"User with ID '{userId}' was activated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg }); 
    }

    [Authorize]
    [HttpPut("{userId}/deactivate")]
    public async Task<IActionResult> DeactivateUser(string userId)
    {
        await _service.DeactivateUser(userId);
        var msg = $"User with ID '{userId}' was deactivated.";
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
