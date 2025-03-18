using Microsoft.AspNetCore.Mvc;
using TemplateNatsApi.Common.Messaging.NATS;
using TemplateNatsApi.Core.Models;

namespace TemplateNatsApi.Core.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : Controller
{
    private readonly NatsPublisher _publisher;
    private readonly ILogger<PingController> _logger;

    public PingController(NatsPublisher publisher, ILogger<PingController> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    [HttpPost("send")]
    public IActionResult SendMessage([FromBody] MessageRequest request)
    {
        _publisher.Publish("dotnet_queue", request);

        var responseMessage = $"Message successfully sent to queue: {request.Name}";
        _logger.LogInformation("Message sent: {Message}", request);
        return Ok(new { message = responseMessage });
    }
}

