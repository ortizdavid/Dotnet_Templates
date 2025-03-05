using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TemplateRabbitMQApi.Common.Messaging.RabbitMQ;

public class RabbitMQConsumer : RabbitMQClientBase
{
    public RabbitMQConsumer(IOptions<RabbitMQSettings> settings) : base(settings) {}

    public async Task ConsumeFromQueue<T>(string queueName)
    {
        await EnsureConnectionAsync();
        await DeclareQueueAsync(queueName);

        Console.WriteLine($" [*] Waiting for messages from queue '{queueName}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                Console.WriteLine($" [x] Received message from queue '{queueName}': {message}");
                await _channel!.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message from queue '{queueName}': {ex.Message}");
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, true);
            }
        };

        await _channel!.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
    }


    public async Task ConsumeFromExchange<T>(string exchangeName, string routingKey)
    {
        await EnsureConnectionAsync();
        await DeclareExchangeAsync(exchangeName);
        var queueDeclareOk = await DeclareAndBindQueueAsync(exchangeName, routingKey);

        Console.WriteLine($" [*] Waiting for messages from exchange '{exchangeName}' with routing key '{routingKey}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                Console.WriteLine($" [x] Received message from exchange '{exchangeName}': {message}");
                await _channel!.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message from exchange '{exchangeName}': {ex.Message}");
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, true);
            }
        };

        await _channel!.BasicConsumeAsync(queue: queueDeclareOk.QueueName, autoAck: false, consumer: consumer);
    }


    public async Task ProcessMessageFromQueue<T>(string queueName, Func<T, Task> processMessage)
    {
        await EnsureConnectionAsync();
        await DeclareQueueAsync(queueName);

        Console.WriteLine($" [*] Waiting for messages from queue '{queueName}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            await HandleMessage(queueName, eventArgs, processMessage);
        };

        await _channel!.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
    }

    public async Task ProcessMessageFromExchange<T>(string exchangeName, string routingKey, Func<T, Task> processMessage)
    {
        await EnsureConnectionAsync();
        await DeclareExchangeAsync(exchangeName);
        var queueDeclareOk = await DeclareAndBindQueueAsync(exchangeName, routingKey);

        Console.WriteLine($" [*] Waiting for messages from exchange '{exchangeName}' with routing key '{routingKey}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
           await HandleMessage(queueDeclareOk.QueueName, eventArgs, processMessage);
        };

        await _channel!.BasicConsumeAsync(queue: queueDeclareOk.QueueName, autoAck: false, consumer: consumer);
    }


    private async Task HandleMessage<T>(string source, BasicDeliverEventArgs eventArgs, Func<T, Task>? processMessage)
    {
        try
        {
            var body = eventArgs.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(messageString);

            if (message is null)
            {
                Console.WriteLine($" [!] Received null or invalid message from '{source}'");
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false);
                return;
            }

            if (processMessage is not null)
            {
                Console.WriteLine($" [x] Processing message from '{source}': {message}");
                await processMessage(message);
            }
        
            await _channel!.BasicAckAsync(eventArgs.DeliveryTag, false);
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($" [!] JSON deserialization error from '{source}': {jsonEx.Message}");
            await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($" [!] Error processing message from '{source}': {ex.Message}");
            await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, true);
        }
    }
}
