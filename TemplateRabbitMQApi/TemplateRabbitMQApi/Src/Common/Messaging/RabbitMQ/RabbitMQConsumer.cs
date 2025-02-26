using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TemplateRabbitMQApi.Core.Messaging.RabbitMQ;

public class RabbitMQConsumer
{
    private readonly RabbitMQSettings _settings;
    
    public RabbitMQConsumer(IOptions<RabbitMQSettings> settings)
    {
        _settings = settings.Value;
    }

    public void ConsumeFromQueue(string queueName)
    {

    }

    public void ConsumeFromExchange(string exchangeName, string routingKey)
    {

    }

    public void ProcessMessageFromQueue<T>(string queueName, Func<T, Task> func)
    {

    }

    public void ProcessMessageFromExchange<T>(string exchangeName, string routingKey, Func<T, Task> func)
    {
        
    }

    public void Close()
    {

    }
}