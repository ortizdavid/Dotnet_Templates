using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace TemplateNatsApi.Common.Messaging.NATS;

public class NatsPublisher : NatsClientBase
{
    private readonly ILogger<NatsPublisher> _logger;

    public NatsPublisher(IOptions<NatsSettings> settings, ILogger<NatsPublisher> logger) : base(settings) 
    {
        _logger = logger;
    }

    public void Publish<T>(string subject, T message)
    {
        try
        {
            EnsureConnection();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            _connection!.Publish(subject, body);
            _logger.LogInformation($"[x] Message published to subject '{subject}': {message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[!] Failed to publish to subject '{subject}': ", ex.Message);
            throw;
        }
    }
}