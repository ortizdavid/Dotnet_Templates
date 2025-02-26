using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TemplateRabbitMQApi.Core.Messaging.RabbitMQ;

public class RabbitMQProducer
{
    private readonly RabbitMQSettings _settings;

    public RabbitMQProducer(IOptions<RabbitMQSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task PublishToQueue<T>(string queueName, T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName, 
            durable: true, 
            exclusive: false,
            autoDelete: false, 
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = new BasicProperties {Persistent = true};
        

    }

    public void PublishToExchange<T>(string exchangeName, string routingKey, T message)
    {
        
    }

    public void Close()
    {

    }
}