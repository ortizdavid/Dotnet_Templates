using Microsoft.AspNetCore.Mvc;

namespace TemplateSimpleApi.Controlers;

[Route("")]
[Route("api")]
[ApiController]
public class ApiRootController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new { Message = "It works!! .NET Simple API" });
    }
}