using System.Net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TemplateSimpleMongoDbApi.Models;
using TemplateSimpleMongoDbApi.Services;

namespace TemplateSimpleMongoDbApi.Controlers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllproducts()
    {
        try
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while retrieving all products");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var product = await _service.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound($"Product with ID '{id}' not found");
            }
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving product with ID '{id}'");
            return StatusCode((int)HttpStatusCode.InternalServerError, $"An error ocurred while retrieving product with ID '{id}'");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]Product newProduct)
    {
        try
        {
            if (newProduct is null)
            {
                _logger.LogError("Create Product request cannot be null");
                return BadRequest("Create Product request cannot be null");
            }
            if (await _service.Exists("Code", newProduct.Code))
            {
                _logger.LogError($"Product with code '{newProduct.Code}' already exists");
                return Conflict($"Product with code '{newProduct.Code}' already exists");
            }
            await _service.CreateAsync(newProduct);
            _logger.LogInformation($"Product '{newProduct.Name}' created successfully");
            return StatusCode((int)HttpStatusCode.Created, new { Message=$"Product '{newProduct.Name}' created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating product");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while creating product");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody]Product updatedProduct)
    {
        try
        {
            if (updatedProduct is null)
            {
                _logger.LogError("Update Product request cannot be null");
                return BadRequest("Update Product request cannot be null");
            }
            var product = await _service.GetByIdAsync(id);
            if (product is null)
            {
                _logger.LogError($"Product with ID '{id}' not found");
                return NotFound($"Product with ID '{id}' not found");
            }
            product.Name = updatedProduct.Name;
            product.Code = updatedProduct.Code;
            product.Price = updatedProduct.Price;
            await _service.UpdateAsync(id, product);
            _logger.LogInformation($"Product with ID '{id}' was updated.");
            return Ok(new {Message=$"Product with ID '{id}' was updated."});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating product");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while creating product");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            var product = await _service.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound($"Product with ID '{id}' not found");
            }
            await _service.DeleteAsync(id);
            _logger.LogInformation($"Product with ID '{id}' was deleted.");
            return Ok(new {Message=$"Product with ID '{id}' was deleted."});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting product");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while deleting product");
        }
    }

}