using System.Net;
using Microsoft.AspNetCore.Mvc;
using TemplateSimpleApi.Models;
using TemplateSimpleApi.Repositories;

namespace TemplateSimpleApi.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private ProductRepository _repository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductRepository repository, ILogger<ProductsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody]Product productReq)
    {
        if (productReq is null)
        {
            _logger.LogError("Create Product request cannot be null");
            return BadRequest("Create Product request cannot be null");
        }
        if (_repository.ExistsRecord("Name", productReq.Name))
        {
            _logger.LogError($"Product '{productReq.Name}' already exists");
            return StatusCode((int)HttpStatusCode.Conflict, $"Product '{productReq.Name}' already exists");
        }
        if (_repository.ExistsRecord("Code", productReq.Code))
        {
            _logger.LogError($"Product code '{productReq.Code}' already exists");
            return StatusCode((int)HttpStatusCode.Conflict, $"Product code '{productReq.Code}' already exists");
        }
        try
        {
            _repository.Create(productReq);
            var msg = $"Product '{productReq.Name}' was created";
            _logger.LogInformation(msg);	
            return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating product");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while creating product");
        }
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        try
        {
            var products = _repository.GetAll();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while retrieving all products");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        try
        {
            var product = _repository.GetById(id);
            if (product is null)
            {
                return NotFound($"Product with ID '{id}' not found");
            }
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID '{id}'", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while retrieving product");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct([FromBody]Product productReq, int id)
    {
        if (productReq is null)
        {
            return BadRequest("Update Product request cannot be null");
        }
        var product = _repository.GetById(id);
        if (product is null)
        {
            return NotFound($"Product with ID '{id}' not found");
        }
        try
        {
            product.Name = productReq.Name;
            product.Code = productReq.Code;
            product.Price = productReq.Price;
            _repository.Update(product);
            var msg = $"Product '{product.Name}' was updated.";
            _logger.LogInformation(msg);
            return Ok(new { Message = msg });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID '{id}'", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while updating product");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            var product = _repository.GetById(id);
            if (product is null)
            {
                return NotFound($"Product with ID '{id}' not found");
            }
            _repository.Delete(product);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID '{id}'", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error ocurred while deleting product");
        }
    }
}