using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TemplateRabbitMQApi.Common.Messaging.RabbitMQ;

public class RabbitMQProducer : RabbitMQClientBase
{
    private readonly ILogger<RabbitMQProducer> _logger;

    public RabbitMQProducer(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQProducer> logger) : base(settings) 
    {
        _logger = logger;
    }

    public async Task PublishToQueue<T>(string queueName, T message)
    {
        try
        {
            await EnsureConnectionAsync();
            await DeclareQueueAsync(queueName);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = new BasicProperties {Persistent = true};

            await _channel!.BasicPublishAsync(
                exchange: string.Empty, 
                routingKey: queueName, 
                mandatory: true,
                basicProperties: properties, 
                body: body
            );
            _logger.LogInformation($"Message published successfully to queue: {queueName}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[!] Failed to publish to queue '{queueName}': ", ex.Message);
            throw;
        }
    }

    public async Task PublishToExchange<T>(string exchangeName, T message, string routingKey)
    {
        try
        {
            await EnsureConnectionAsync();
            await DeclareExchangeAsync(exchangeName);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = new BasicProperties {Persistent = true};

            await _channel!.BasicPublishAsync(
                exchange: exchangeName, 
                routingKey: routingKey, 
                mandatory: true,
                basicProperties: properties, 
                body: body
            );
            _logger.LogInformation($"Message published successfully to exchange: {exchangeName} with routing key: {routingKey}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[!] Failed to publish to exchange '{exchangeName}' with routing key: {routingKey}: ", ex.Message);
            throw;
        }
    }
}