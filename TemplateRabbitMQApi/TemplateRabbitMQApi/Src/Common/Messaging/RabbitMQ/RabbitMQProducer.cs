using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TemplateRabbitMQApi.Common.Messaging.RabbitMQ;

public class RabbitMQProducer : RabbitMQClientBase
{
    public RabbitMQProducer(IOptions<RabbitMQSettings> settings) : base(settings) {}

    public async Task PublishToQueue<T>(string queueName, T message)
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
    }

    public async Task PublishToExchange<T>(string exchangeName, T message, string routingKey, ExchangeType exchangeType = ExchangeType.Direct)
    {
        await EnsureConnectionAsync();
        await DeclareExchangeAsync(exchangeName, exchangeType);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var properties = new BasicProperties {Persistent = true};

        await _channel!.BasicPublishAsync(
            exchange: exchangeName, 
            routingKey: routingKey, 
            mandatory: true,
            basicProperties: properties, 
            body: body
        );
    }

}