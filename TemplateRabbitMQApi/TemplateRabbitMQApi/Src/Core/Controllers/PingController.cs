using Microsoft.AspNetCore.Mvc;
using TemplateRabbitMQApi.Common.Messaging.RabbitMQ;
using TemplateRabbitMQApi.Core.Models;

namespace TemplateRabbitMQApi.Core.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : Controller
{
    private readonly RabbitMQProducer _producer;
    private readonly ILogger<PingController> _logger;

    public PingController(RabbitMQProducer producer, ILogger<PingController> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
    {
        var jsonMessage = request;
        await _producer.PublishToQueue("dotnet_queue", jsonMessage);

        var responseMessage = $"Message successfully sent to queue: {request.Name}";
        _logger.LogInformation("Message sent: {Message}", request);
        return Ok(new { message = responseMessage });
    }
}

