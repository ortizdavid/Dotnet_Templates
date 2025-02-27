using Microsoft.AspNetCore.Mvc;

namespace TemplateRabbitMQApi.Core.Controllers;

[ApiController]
[Route("api")]
public class ApiRootController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Template RabbitMQ API");
    }
}