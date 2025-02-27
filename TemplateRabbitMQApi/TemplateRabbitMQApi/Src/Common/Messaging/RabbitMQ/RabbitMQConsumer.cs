using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TemplateRabbitMQApi.Common.Messaging.RabbitMQ;

public class RabbitMQConsumer : RabbitMQClientBase
{   
    public RabbitMQConsumer(IOptions<RabbitMQSettings> settings) : base(settings) {}

    public async Task ConsumeFromQueue(string queueName)
    {

    }

    public async Task ConsumeFromExchange(string exchangeName, string routingKey)
    {

    }

    public async Task ProcessMessageFromQueue<T>(string queueName, Func<T, Task> func)
    {

    }

    public async Task ProcessMessageFromExchange<T>(string exchangeName, string routingKey, Func<T, Task> func)
    {
        
    }
}