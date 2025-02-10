using TemplateMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Services.Auth;
using TemplateMVC.Common.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TemplateMVC.Core.Controllers.Auth;

public class UsersController : Controller
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
        try
        {
            var users = await _service.GetAllUsers(param);
            return Ok(users);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel)
    {
        try
        {
            await _service.CreateUser(viewModel);
            var msg = $"User '{viewModel.UserName}' was created";
            _logger.LogInformation(msg);
            return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
       
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetUserById(Guid uniqueId)
    {
        try
        {
            var user = await _service.GetUserByUniqueId(uniqueId);
            return Ok(user);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpGet("by-name/{userName}")]
    public async Task<IActionResult> GetUserByName(string userName)
    {
        try
        {
            var user = await _service.GetUserByName(userName);
            return Ok(user);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpPut("{uniqueId}/upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file, Guid uniqueId)
    {
        try
        {
            await _service.UploadUserImage(file, uniqueId);
            var msg = $"User '{uniqueId}' image uploaded.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpPut("{uniqueId}/change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel, Guid uniqueId)
    {
        try
        {
            await _service.ChangePassword(viewModel, uniqueId);
            var msg = $"User '{uniqueId}' password was changed";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteUser(Guid uniqueId)
    {
        try
        {
            await _service.DeleteUser(uniqueId);
            _logger.LogInformation($"User with ID '{uniqueId}' was deleted");
            return NoContent();
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpPut("{uniqueId}/activate")]
    public async Task<IActionResult> ActivateUser(Guid uniqueId)
    {
        try
        {
            await _service.ActivateUser(uniqueId);
            var msg = $"User with ID '{uniqueId}' was activated.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{uniqueId}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid uniqueId)
    {
        try
        {
            await _service.DeactivateUser(uniqueId);
            var msg = $"User with ID '{uniqueId}' was deactivated.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var user = await _authService.GetLoggedUser();
            return Ok(user);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }
}
