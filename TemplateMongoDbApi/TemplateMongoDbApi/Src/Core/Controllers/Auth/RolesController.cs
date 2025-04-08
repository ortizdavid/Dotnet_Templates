using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMongoDbApi.Common.Helpers;
using TemplateMongoDbApi.Core.Models.Auth;
using TemplateMongoDbApi.Core.Services.Auth;

namespace TemplateMongoDbApi.Core.Controllers.Auth;

//[Authorize]
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
        var roles = await _service.GetAllRoles(param);
        return Ok(roles);   
    }

    [HttpGet("not-paginated")]
    public async Task<IActionResult> GetRolesNotPaginated()
    {
        var roles = await _service.GetRolesNotPaginated();
        return Ok(roles);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRole([FromBody]RoleRequest request)
    {
        await _service.CreateRole(request);
        var msg = $"Role '{request.RoleName}' created";
        _logger.LogInformation(msg);
        return StatusCode((int)HttpStatusCode.Created, new{ Message = msg });
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateRole([FromBody]RoleRequest request, string roleId)
    {
        await _service.UpdateRole(request, roleId);
        var msg = $"Role with ID '{roleId}' updated";
        _logger.LogInformation(msg);
        return Ok(new{ Message = msg });
    }

    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetRoleByUniqueId(string roleId)
    {
        var role = await _service.GetRoleById(roleId);
        return Ok(role);
    }

    [HttpGet("by-code/{code}")]
    public async Task<IActionResult> GetRoleByCode(string code)
    {
        var role = await _service.GetRoleByCode(code);
        return Ok(role);
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        await _service.DeleteRole(roleId);
        _logger.LogInformation($"Role with ID '{roleId}' deleted");
        return NoContent();
    }
}