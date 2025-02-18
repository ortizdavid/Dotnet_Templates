using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Products;
using TemplateMVC.Helpers;
using TemplateMVC.Common.Exceptions;
using System.Net;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Controllers.Products;

[Route("categories")]
public class CategoriesController : Controller
{
    private readonly CategoryService _service;
    private readonly ILogger<CategoriesController> _logger;
    private readonly IHttpContextAccessor _contextAcessor;
    private HttpContext? _context => _contextAcessor?.HttpContext;

    
    public CategoriesController(CategoryService service, ILogger<CategoriesController> logger, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _logger = logger;
        _contextAcessor = contextAccessor;
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

            var categories = await _service.GetAllCategories(param, filter);
            return View(categories);
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
    public async Task<IActionResult> Create(CreateCategoryViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }   
            await _service.CreateCategory(viewModel);
            _logger.LogInformation($"Category '{viewModel.CategoryName}' was created.");
            return Redirect("/categories");
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
        var category = await _service.GetCategoryByUniqueId(uniqueId);
        var model = new UpdateCategoryViewModel()
        {
            CategoryName = category.CategoryName,
            Description = category.Description,
            UniqueId = category.UniqueId
        };
        return View(model);
    }


    [HttpPost("{uniqueId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateCategoryViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.UpdateCategory(viewModel, uniqueId);
            _logger.LogInformation($"Category '{viewModel.CategoryName}' was updated.");
            return Redirect($"/categories/{uniqueId}/details");
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
            var category = await _service.GetCategoryByUniqueId(uniqueId);
            return View(category);
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();  
        }
    }

    [HttpGet("{uniqueId}/delete")]
    public async Task<IActionResult> Delete(Guid uniqueId)
    {
        var category = await _service.GetCategoryByUniqueId(uniqueId);
        var model = new DeleteCategoryViewModel()
        {
            UniqueId = category.UniqueId, 
            CategoryName = category.CategoryName
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel viewModel, Guid uniqueId)
    {
        try
        {
            await _service.DeleteCategory(uniqueId);
            _logger.LogInformation($"Category deleted.");
            return Redirect("/categories");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();  
        }
    }

    [HttpGet("import-csv")]
    public IActionResult ImportCsv()
    {
        return View();
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("file", "Please select a CSV file.");
            return View();
        }
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.ImportCategoriesCSV(file);
            _logger.LogInformation($"Categories imported by CSV successfully");
            return Redirect("/categories");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();  
        }
    }
}

