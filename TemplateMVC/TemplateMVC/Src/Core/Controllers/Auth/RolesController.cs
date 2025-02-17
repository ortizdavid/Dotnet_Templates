using System.Net;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Services.Auth;
using System.Threading.Tasks;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Controllers.Auth;

[Route("roles")]
public class RolesController : Controller
{
    private readonly RoleService _service;
    private readonly ILogger<RolesController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private HttpContext? _context => _contextAccessor?.HttpContext;

    public RolesController(RoleService service, ILogger<RolesController> logger, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index(SearchFilter filter)
    {
        try
        {
            var param = PaginationParam.GetFromContext(_context);
            ViewBag.PaginationParam = param;
            ViewBag.CurrentSearch = filter.SearchString;
            ViewBag.CurrentSort = filter.SortOrder;
            ViewBag.NameSort = (filter.SortOrder == "name_desc") ? "name_asc" : "name_desc";
            ViewBag.CodeSort = (filter.SortOrder == "code_desc") ? "code_asc" : "code_desc";

            var roles = await _service.GetAllRoles(param, filter);
            return View(roles);
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.CreateRole(viewModel);
            _logger.LogInformation($"Role '{viewModel.RoleName}' created");
            return Redirect("/roles");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/edit")]
    public async Task<IActionResult> Edit(Guid uniqueId)
    {
        var role = await _service.GetRoleByUniqueId(uniqueId);
        var model = new UpdateRoleViewModel()
        {
            RoleName = role.RoleName,
            Code = role.Code,
            UniqueId = role.UniqueId
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateRoleViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.UpdateRole(viewModel, uniqueId);
            _logger.LogInformation($"Role with ID '{uniqueId}' updated");
            return Redirect($"/roles/{uniqueId}/details");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/details")]
    public async Task<IActionResult> Details(Guid uniqueId)
    {
        try
        {
            var role = await _service.GetRoleByUniqueId(uniqueId);
            return View(role);
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
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
            ModelState.AddModelError("", ex.Message);
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> Delete(Guid uniqueId)
    {
        try
        {
            await _service.DeleteRole(uniqueId);
            _logger.LogInformation($"Role with ID '{uniqueId}' deleted");
            return NoContent();
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return StatusCode(ex.StatusCode, new { Message = ex.Message });
        }
    }
}