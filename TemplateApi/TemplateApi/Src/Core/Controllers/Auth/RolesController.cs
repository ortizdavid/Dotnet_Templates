using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Common.Exceptions;
using TemplateApi.Helpers;
using TemplateApi.Core.Models.Auth;
using TemplateApi.Core.Services.Auth;

namespace TemplateApi.Core.Controllers.Auth;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleService _service;
    private readonly ILogger<RolesController> _logger;

    public RolesController(RoleService service, ILogger<RolesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles([FromQuery]PaginationParam param)
    {
        try
        {
            var roles = await _service.GetAllRoles(param);
            return Ok(roles);
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

    [HttpGet("not-paginated")]
    public async Task<IActionResult> GetRolesNotPaginated()
    {
        try
        {
            var roles = await _service.GetRolesNotPaginated();
            return Ok(roles);
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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRole([FromBody]RoleRequest request)
    {
        try
        {
            await _service.CreateRole(request);
            var msg = $"Role '{request.RoleName}' created";
            _logger.LogInformation(msg);
            return StatusCode((int)HttpStatusCode.Created, new{ Message = msg });
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

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateRole([FromBody]RoleRequest request, Guid uniqueId)
    {
        try
        {
            await _service.UpdateRole(request, uniqueId);
            var msg = $"Role with ID '{uniqueId}' updated";
            _logger.LogInformation(msg);
            return Ok(new{ Message = msg });
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

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetRoleByUniqueId(Guid uniqueId)
    {
        try
        {
            var role = await _service.GetRoleByUniqueId(uniqueId);
            return Ok(role);
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

    [HttpGet("by-code/{code}")]
    public async Task<IActionResult> GetRoleByCode(string code)
    {
        try
        {
            var role = await _service.GetRoleByCode(code);
            return Ok(role);
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

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteRole(Guid uniqueId)
    {
        try
        {
            await _service.DeleteRole(uniqueId);
            _logger.LogInformation($"Role with ID '{uniqueId}' deleted");
            return NoContent();
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