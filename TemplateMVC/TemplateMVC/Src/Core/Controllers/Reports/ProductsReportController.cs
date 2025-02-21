using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Services.Reports;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Core.Services.Products;
using TemplateMVC.Core.Services.Suppliers;

namespace TemplateMVC.Core.Controllers.Reports;

[Route("reports/products")]
public class ProductsReportController : Controller
{
    private readonly ProductReportService _service;
    private readonly CategoryService _categoryService;
    private readonly SupplierService _supplierService;
    private readonly IGenerator<ProductReport> _generator;
    private readonly ILogger<CategoriesReportController> _logger;

    public ProductsReportController(ProductReportService service, CategoryService categoryService, SupplierService supplierService, ILogger<CategoriesReportController> logger)
    {
        _service = service;
        _categoryService = categoryService;
        _supplierService = supplierService;
        _generator = new ProductGenerator();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Reports/Products/Index.cshtml");
    }

    [HttpGet("all-products")]
    public IActionResult AllProducts()
    {
        return View("~/Views/Reports/Products/AllProducts.cshtml");
    }

    [HttpPost("all-products")]
    public async Task<IActionResult> AllProducts(ReportFilter filter)
    {
        try
        {
            var products = await _service.GetAllProducts(filter);
            return ReportFormat.Handle(_generator, products, filter.Format, "All_Products");
        }
        catch (AppException ex) 
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("by-categories")]
    public async Task<IActionResult> ProductsByCategories()
    {
        var filter = new ProductReportFilter()
        {
            Categories = await _categoryService.GetAllNotPaginated()
        };
        return View("~/Views/Reports/Products/ProductsByCategories.cshtml", filter);
    }

    [HttpPost("by-categories")]
    public async Task<IActionResult> ProductsByCategories(ProductReportFilter filter)
    {
        try
        {
            var products = await _service.GetAllProductsByCategory(filter);
            return ReportFormat.Handle(_generator, products, filter.Format, "Products_By_Categories");
        }
        catch (AppException ex) 
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("by-suppliers")]
    public async Task<IActionResult> ProductsBySuppliers()
    {
        var filter = new ProductReportFilter()
        {
            Suppliers = await _supplierService.GetAllNotPaginated()
        };
        return View("~/Views/Reports/Products/ProductsBySuppliers.cshtml", filter);
    }

    [HttpPost("by-suppliers")]
    public async Task<IActionResult> ProductsBySuppliers(ProductReportFilter filter)
    {
        try
        {
            var products = await _service.GetAllProductsBySupplier(filter);
            return ReportFormat.Handle(_generator, products, filter.Format, "Products_By_Suppliers");
        }
        catch (AppException ex) 
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}
