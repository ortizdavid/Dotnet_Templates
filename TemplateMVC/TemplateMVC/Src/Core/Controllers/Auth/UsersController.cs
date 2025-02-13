using TemplateMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Services.Auth;
using TemplateMVC.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Controllers.Auth;

[Route("users")]
public class UsersController : Controller
{
    private readonly UserService _service;
    private readonly AuthService _authService;
    private readonly RoleService _roleService;
    private readonly ILogger<UsersController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    private HttpContext? _context => _contextAccessor.HttpContext;

    public UsersController(UserService service, AuthService authService, RoleService roleService, ILogger<UsersController> logger, IHttpContextAccessor contextAccessor) 
    {
        _service = service;
        _authService = authService;
        _roleService = roleService;
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
            ViewBag.RoleSort = (filter.SortOrder == "role_desc") ? "role_asc" : "role_desc";;
          
            var users = await _service.GetAllUsers(param, filter);
            return View(users);
        }
        catch (AppException ex)
        {
            ViewBag.ErrorMessage =  ex.Message;
            return View();
        }
    }

    [HttpGet("create")]
    public async Task<IActionResult> CreateUser()
    {
        var model = new CreateUserViewModel()
        {
            Roles = await _roleService.GetRolesNotPaginated()
        };
        return View(model);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel)
    {
        await PopulateRolesCreateAsync(viewModel);
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.CreateUser(viewModel);
            _logger.LogInformation($"User '{viewModel.UserName}' was created");
            return RedirectToAction("Index");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            await PopulateRolesCreateAsync(viewModel);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/edit")]
    public async Task<IActionResult> EditUser(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
        if (user is null)
        {
            ViewBag.ErrorMessage = $"User with ID '{uniqueId}' not found";
            return View();
        }
        var model = new UpdateUserViewModel()
        {
            UniqueId = uniqueId,
            UserName = user.UserName,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.RoleName,
            Roles = await _roleService.GetRolesNotPaginated()
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UpdateUserViewModel viewModel, Guid uniqueId)
    {
        await PopulateRolesUpdateAsync(viewModel);
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.UpdateUser(viewModel, uniqueId);
            _logger.LogInformation($"User '{viewModel.UserName}' was edited");
            return Redirect($"/users/{uniqueId}/details");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            await PopulateRolesUpdateAsync(viewModel);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/details")]
    public async Task<IActionResult> Details(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
        return View(user);
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

    private async Task PopulateRolesCreateAsync(CreateUserViewModel viewModel)
    {
        viewModel.Roles = await _roleService.GetRolesNotPaginated();
    }

    private async Task PopulateRolesUpdateAsync(UpdateUserViewModel viewModel)
    {
        viewModel.Roles = await _roleService.GetRolesNotPaginated();
    }
}
