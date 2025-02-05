using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TemplateSimpleMVC.Helpers;
using TemplateSimpleMVC.Models;
using TemplateSimpleMVC.Repositories;

namespace TemplateSimpleMVC.Controllers;

public class ProductsController : Controller
{
    private readonly ProductRepository _repository;
    private readonly UserContext _userContext;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductRepository repository, UserContext userContext, ILogger<ProductsController> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int pageNumber=1)
    {
        ViewBag.CurrentSort = sortOrder;
        ViewBag.CurrentFilter = searchString;
        ViewBag.NameSort = (sortOrder == "name_desc") ? "name" : "name_desc"; // Toogling the sort order
        ViewBag.CodeSort = (sortOrder == "code_desc") ? "code" : "code_desc";
    
        int pageSize = 10;
        if (searchString != currentFilter) pageNumber = 1;

        var products = _repository.GetAllSorted(sortOrder, searchString);
        var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize);
        return View(products);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            ViewBag.ErrorMessage = $"Product with id '{id}' not found";
            return View();
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (_repository.Exists("Code", model.Code))
        {
            var msg = $"Product with code '{model.Code}' already exists";
            _logger.LogError(msg);
            ModelState.AddModelError("", msg);
            return View(model);
        }
        _repository.Create(model);
        _logger.LogInformation($"Product '{model.Name}' created successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            ViewBag.ErrorMessage = $"Product with id '{id}' not found";
            return View();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product model, int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            ViewBag.ErrorMessage = $"Product with id '{id}' not found";
            return View(nameof(Edit));
        }
        if (!ModelState.IsValid)
        {
           return View(model); 
        }
        product.Name = model.Name;
        product.Code = model.Code;
        product.Price = model.Price;
        product.Description = model.Description;
        _repository.Update(product);
        _logger.LogInformation($"Product with id '{id}' edited");
        return Redirect("/Products/Details/"+id);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            ViewBag.ErrorMessage = $"Product with id '{id}' not found";
            return View();
        }
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmDelete(int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            ViewBag.ErrorMessage = $"Product with id '{id}' not found";
            return View(nameof(Delete));
        }
        _repository.Delete(product);
        _logger.LogInformation("Product Deleted");
        return RedirectToAction(nameof(Index));
    }
}