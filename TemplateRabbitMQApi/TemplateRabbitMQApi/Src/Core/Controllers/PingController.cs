using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TemplateRabbitMQApi.Common.Messaging.RabbitMQ;

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

        _logger.LogInformation($"Message Sent: '{request.ToString()}'");
        return Ok(request);
    }
}

public class MessageRequest
{
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Identification { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Name: {Name}\nGender: {Gender}\nIdentification: {Identification}";
    }
}