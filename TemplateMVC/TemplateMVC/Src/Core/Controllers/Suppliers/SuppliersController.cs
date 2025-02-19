using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Core.Services.Suppliers;
using TemplateMVC.Helpers;
using TemplateMVC.Common.Exceptions;
using System.Net;
using TemplateMVC.Common.Helpers;
using TemplateMVC.Core.Models.Suppliers;
using System.Threading.Tasks;

namespace TemplateMVC.Core.Controllers.Suppliers;

[Route("suppliers")]
public class SuppliersController : Controller
{
    private readonly SupplierService _service;
    private readonly ILogger<SuppliersController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private HttpContext? _context => _contextAccessor?.HttpContext;

    public SuppliersController(ILogger<SuppliersController> logger, SupplierService service, IHttpContextAccessor contextAccessor)
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
            ViewBag.IdentSort = (filter.SortOrder == "ident_desc") ? "ident_asc" : "indent_desc";;

            var suppliers = await _service.GetAllSuppliers(param, filter);
            return View(suppliers);
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
    public async Task<IActionResult> Create(CreateSupplierViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.CreateSupplier(viewModel);
            _logger.LogInformation($"Supplier '{viewModel.SupplierName}' created.");
            return Redirect("/suppliers");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/details")]
    public async Task<IActionResult> Details(Guid uniqueId)
    {
        try 
        {
            var supplier = await _service.GetSupplierByUniqueId(uniqueId);
            return View(supplier);
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
        var supplier = await _service.GetSupplierByUniqueId(uniqueId);
        var model = new UpdateSupplierViewModel()
        {
            SupplierName = supplier.SupplierName,
            IdentificationNumber = supplier.IdentificationNumber,
            Email = supplier.Email,
            PrimaryPhone = supplier.PrimaryPhone,
            SecondaryPhone = supplier.SecondaryPhone,
            Address = supplier.Address,
            UniqueId = supplier.UniqueId
        };
        return View(model);
    }

    [HttpPost("{uniqueId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateSupplierViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.UpdateSupplier(viewModel, uniqueId);
            var msg = $"Supplier '{viewModel.SupplierName}' updated.";
            _logger.LogInformation(msg);
            return Redirect($"/suppliers/{uniqueId}/details");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/delete")]
    public async Task<IActionResult> Delete(Guid uniqueId)
    {
        var supplier = await _service.GetSupplierByUniqueId(uniqueId);
        var model = new DeleteSupplierViewModel()
        {
            UniqueId = supplier.UniqueId,
            SupplierName = supplier.SupplierName,
            IdentificationNumber = supplier.IdentificationNumber
        };
        return View(model);
    }
    
    [HttpPost("{uniqueId}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeleteSupplierViewModel viewModel, Guid uniqueId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.DeleteSupplier(uniqueId);
            _logger.LogInformation($"Supplier with ID '{uniqueId}' deleted.");
            return Redirect($"/suppliers");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet("{uniqueId}/products")]
    public async Task<IActionResult> GetSupplierProducts(Guid uniqueId)
    {
        try
        {
            var products = await _service.GetSupplierProducts(uniqueId);
            return Ok(products);
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
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
            await _service.ImportSuppliersCSV(file);
            _logger.LogInformation($"Suppliers imported by CSV successfully");
            return Redirect("/suppliers");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}
