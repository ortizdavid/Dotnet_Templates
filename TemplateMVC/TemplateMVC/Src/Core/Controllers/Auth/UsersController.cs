using TemplateMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Services.Auth;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Controllers.Auth;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _service;
    private readonly AuthService _authService;
    private readonly RoleService _roleService;
    private readonly ILogger<UsersController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private HttpContext? _context => _contextAccessor.HttpContext;

    public UsersController(IUserService service, AuthService authService, RoleService roleService, ILogger<UsersController> logger, IHttpContextAccessor contextAccessor) 
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
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var model = new CreateUserViewModel()
        {
            Roles = await _roleService.GetRolesNotPaginated()
        };
        return View(model);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel viewModel)
    {
        await _roleService.PopulateToCreateUserAsync(viewModel);
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
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/edit")]
    public async Task<IActionResult> Edit(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
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
    public async Task<IActionResult> Edit(UpdateUserViewModel viewModel, Guid uniqueId)
    {
        await _roleService.PopulateToUpdateUserAsync(viewModel);
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
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/details")]
    public async Task<IActionResult> Details(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
        return View(user);
    }

    [HttpPost("{uniqueId}/upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file, Guid uniqueId)
    {
        try
        {
            await _service.UploadUserImage(file, uniqueId);
            var msg = $"User '{uniqueId}' image uploaded.";
            _logger.LogInformation(msg);
            return View();
        }
         catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpPost("{uniqueId}/change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel, Guid uniqueId)
    {
        try
        {
            await _service.ChangePassword(viewModel, uniqueId);
            var msg = $"User '{uniqueId}' password was changed";
            _logger.LogInformation(msg);
            return View();
        }
         catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpPost("{uniqueId}")]
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
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/activate")]
    public async Task<IActionResult> ActivateUser(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
        var model = new ActivateUserViewModel()
        {
            UniqueId = user.UniqueId,
            UserName = user.UserName,
            Role = user.RoleName
        };
        return View(model);
    }


    [HttpPost("{uniqueId}/activate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(ActivateUserViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var loggedUser = await _authService.GetLoggedUser();
            await _service.ActivateUser(uniqueId, loggedUser?.UniqueId);
            _logger.LogInformation($"User with ID '{uniqueId}' was activated.");
            return Redirect($"/users/{uniqueId}/details");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid uniqueId)
    {
        var user = await _service.GetUserByUniqueId(uniqueId);
        var model = new DectivateUserViewModel()
        {
            UniqueId = user.UniqueId,
            UserName = user.UserName,
            Role = user.RoleName
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/deactivate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(DectivateUserViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var loggedUser = await _authService.GetLoggedUser();
            await _service.DeactivateUser(uniqueId, loggedUser?.UniqueId);
            _logger.LogInformation($"User with ID '{uniqueId}' was deactivated.");
            return Redirect($"/users/{uniqueId}/details");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }
}
