using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Services.Products;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Controllers.Products;

[Route("products")]
public class ProductsController : Controller
{
    private readonly ProductService _service;
    private readonly ILogger<ProductsController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    private HttpContext? _context => _contextAccessor?.HttpContext;
    
    public ProductsController(ProductService service, ILogger<ProductsController> logger, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts(SearchFilter filter)
    {
        try
        {
            var param = PaginationParam.GetFromContext(_context);
            var products = await _service.GetAllProducts(param, filter);
            return View(products);
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
    public async Task<IActionResult> Create(CreateProductViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.CreateProduct(viewModel);
            _logger.LogInformation($"Product '{viewModel.ProductName}' created.");
            return Redirect("/products");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/edit")]
    public IActionResult Edit(Guid uniqueId)
    {
        return View();
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> Edit(UpdateProductViewModel viewModel, Guid uniqueId)
    {
        try
        {
            await _service.UpdateProduct(viewModel, uniqueId);
            var msg = $"Product '{uniqueId}' updated.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetProductByUniqueId(Guid uniqueId)
    {
        try 
        {
            var product = await _service.GetProductByUniqueId(uniqueId);
            return Ok(product);
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteProduct(Guid uniqueId)
    {
        try
        {
            await _service.DeleteProduct(uniqueId);
            _logger.LogInformation($"Product '{uniqueId}' deleteted.");
            return NoContent();
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpPost("{uniqueId}/images")]
    public async Task<IActionResult> UploadProductImages(Guid uniqueId, IFormFileCollection files)
    {
        try
        {
            await _service.UploadProductImages(uniqueId, files);
            var msg = $"Product '{uniqueId}' images uploaded.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("{uniqueId}/images")]
    public async Task<IActionResult> GetProductImages(Guid uniqueId)
    {
        try
        {
            var images = await _service.GetProductImages(uniqueId);
            return Ok(images);
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        try
        {
            await _service.ImportProductsCSV(file);
            var msg = $"Products imported by CSV successfully";
            _logger.LogInformation(msg);
            return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
        }
        catch (AppException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}

