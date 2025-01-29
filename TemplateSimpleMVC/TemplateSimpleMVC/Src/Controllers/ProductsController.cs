using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Index()
    {
        var products = _repository.GetAll();
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
    [ValidateAntiForgeryToken]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
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
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product model, int id)
    {
        return Redirect("/Product/Details"+id);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _repository.GetById(id);
        return View(product);
    }

    [HttpDelete]
    public IActionResult ProcessDelete(int id)
    {
        return RedirectToAction(nameof(Index));
    }
}