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
            return BadRequest("Create Product request cannot be null");
        }
        if (_repository.ExistsRecord("Name", productReq.Name))
        {
            return StatusCode((int)HttpStatusCode.Conflict, $"Product '{productReq.Name}' already exists");
        }
        if (_repository.ExistsRecord("Code", productReq.Code))
        {
            return StatusCode((int)HttpStatusCode.Conflict, $"Product code '{productReq.Code}' already exists");
        }
        _repository.Create(productReq);
        var msg = $"Product '{productReq.Name}' was created";
        _logger.LogInformation(msg);	
        return StatusCode((int)HttpStatusCode.Created, new { Message = msg });
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _repository.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            return NotFound($"Product with ID '{id}' not found");
        }
        return Ok(product);
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
        product.Name = productReq.Name;
        product.Code = productReq.Code;
        product.Price = productReq.Price;
        _repository.Update(product);
        var msg = $"Product '{product.Name}' was updated.";
        _logger.LogInformation(msg);
        return Ok(new { Message = msg });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _repository.GetById(id);
        if (product is null)
        {
            return NotFound($"Product with ID '{id}' not found");
        }
        _repository.Delete(product);
        return NoContent();
    }
}