using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Services.Products;
using TemplateMVC.Common.Helpers;
using TemplateMVC.Core.Services.Suppliers;

namespace TemplateMVC.Core.Controllers.Products;

[Route("products")]
public class ProductsController : Controller
{
    private readonly ProductService _service;
    private readonly CategoryService _categoryService;
    private readonly SupplierService _supplierService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    private HttpContext? _context => _contextAccessor?.HttpContext;
    
    public ProductsController(ProductService service, CategoryService categoryService, SupplierService supplierService,
        ILogger<ProductsController> logger, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _categoryService = categoryService;
        _supplierService = supplierService;
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
            ViewBag.CategorySort = (filter.SortOrder == "category_desc") ? "category_asc" : "category_desc";
            ViewBag.SupplierSort = (filter.SortOrder == "supplier_desc") ? "supplier_asc" : "supplier_desc";

            var products = await _service.GetAllProducts(param, filter);
            return View(products);
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
        var model = new CreateProductViewModel()
        {
            Categories = await _categoryService.GetAllNotPaginated(),
            Suppliers = await _supplierService.GetAllNotPaginated()
        };
        return View(model);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel viewModel)
    {
        await _categoryService.PopulateToCreateProuduct(viewModel);
        await _supplierService.PopulateToCreateProuduct(viewModel);
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.CreateProduct(viewModel);
            _logger.LogInformation($"Product '{viewModel.ProductName}' created.");
            return Redirect("/products");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/edit")]
    public async Task<IActionResult> Edit(Guid uniqueId)
    {
        var product = await _service.GetProductByUniqueId(uniqueId);
        var model = new UpdateProductViewModel()
        {
            ProductName = product.ProductName,
            Code = product.Code,
            UnitPrice = product.UnitPrice,
            Description = product.Description,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            UniqueId = product.UniqueId,
            Categories = await _categoryService.GetAllNotPaginated(),
            Suppliers = await _supplierService.GetAllNotPaginated()
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductViewModel viewModel, Guid uniqueId)
    {
        await _categoryService.PopulateToUpdateProduct(viewModel);
        await _supplierService.PopulateToUpdateProduct(viewModel);
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.UpdateProduct(viewModel, uniqueId);
            var msg = $"Product '{uniqueId}' updated.";
            _logger.LogInformation(msg);
            return Redirect($"/products/{uniqueId}/details");
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
        try 
        {
            var detailsViewModel = new ProductDetailsViewModel()
            {
                Product = await _service.GetProductByUniqueId(uniqueId),
                Images = await _service.GetProductImages(uniqueId)
            };
            return View(detailsViewModel);
        }
         catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/delete")]
    public async Task<IActionResult> Delete(Guid uniqueId)
    {
        var product = await _service.GetProductByUniqueId(uniqueId);
        var model = new DeleteProductViewModel()
        {
            ProductName = product.ProductName,
            Code = product.Code,
            UnitPrice = product.UnitPrice,
            UniqueId = product.UniqueId
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeleteProductViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.DeleteProduct(uniqueId);
            _logger.LogInformation($"Product with ID '{uniqueId}' deleted.");
            return Redirect("/products");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/upload-images")]
    public async Task<IActionResult> UploadImage(Guid uniqueId)
    {
        var product = await _service.GetProductByUniqueId(uniqueId);
        var model = new UploadProductImageViewModel()
        {
            ProductName = product.ProductName,
            UniqueId = product.UniqueId
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/upload-images")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadImage(UploadProductImageViewModel viewModel, Guid uniqueId, IFormFileCollection files)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _service.UploadProductImages(uniqueId, files);
            _logger.LogInformation($"Product '{uniqueId}' images uploaded.");
            return Redirect($"/products/{uniqueId}/details");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet("import-csv")]
    public IActionResult ImportCSV()
    {
        return View();
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCSV(IFormFile file)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();   
            }
            await _service.ImportProductsCSV(file);
            _logger.LogInformation($"Products imported by CSV successfully");
            return Redirect("/products");
        }
        catch (AppException ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}
