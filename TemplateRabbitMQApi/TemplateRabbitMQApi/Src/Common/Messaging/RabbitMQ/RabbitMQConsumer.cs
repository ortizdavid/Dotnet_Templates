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

        Console.WriteLine($" [*] Waiting for messages to consume from queue '{queueName}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                Console.WriteLine($" [x] Received message from queue '{queueName}': {message}");
                await _channel!.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error consuming message from queue '{queueName}': {ex.Message}");
                await _channel!.BasicRejectAsync(deliveryTag: eventArgs.DeliveryTag, requeue: true);
            }
        };

        await _channel!.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        Console.ReadKey();
    }


    public async Task ConsumeFromExchange<T>(string exchangeName, string routingKey)
    {
        await EnsureConnectionAsync();
        await DeclareExchangeAsync(exchangeName);
        var queueDeclareOk = await DeclareAndBindQueueAsync(exchangeName, routingKey);

        Console.WriteLine($" [*] Waiting for messages to consume from exchange '{exchangeName}' with routing key '{routingKey}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                Console.WriteLine($" [x] Received message from exchange '{exchangeName}': {message}");
                await _channel!.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error consuming message from exchange '{exchangeName}': {ex.Message}");
                await _channel!.BasicRejectAsync(deliveryTag: eventArgs.DeliveryTag, requeue: true);
            }
        };

        await _channel!.BasicConsumeAsync(queue: queueDeclareOk.QueueName, autoAck: false, consumer: consumer);
        Console.ReadKey();
    }


    public async Task ProcessMessageFromQueue<T>(string queueName, Func<T, Task> funcProcessMessage)
    {
        await EnsureConnectionAsync();
        await DeclareQueueAsync(queueName);

        Console.WriteLine($" [*] Waiting for messages to Process from queue '{queueName}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                if (message is not null)
                {
                    await funcProcessMessage(message);
                    await _channel!.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                    Console.WriteLine($" [x] Message processed successfully: {message} via '{funcProcessMessage.Method.Name}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message from queue '{queueName}': {ex.Message}");
                await _channel!.BasicRejectAsync(deliveryTag: eventArgs.DeliveryTag, requeue: true);
            }
        };

        await _channel!.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        Console.ReadKey();
    }


    public async Task ProcessMessageFromExchange<T>(string exchangeName, string routingKey, Func<T, Task> funcProcessMessage)
    {
        await EnsureConnectionAsync();
        await DeclareExchangeAsync(exchangeName);
        var queueDeclareOk = await DeclareAndBindQueueAsync(exchangeName, routingKey);

        Console.WriteLine($" [*] Waiting for messages to Process from exchange '{exchangeName}' with routing key '{routingKey}'.");

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
           try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));
                if (message is not null)
                {
                    await funcProcessMessage(message);
                    await _channel!.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                    Console.WriteLine($" [x] Message processed successfully: {message} via '{funcProcessMessage.Method.Name}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message from exchange '{exchangeName}': {ex.Message}");
                await _channel!.BasicRejectAsync(deliveryTag: eventArgs.DeliveryTag, requeue: true);
            }
        };

        await _channel!.BasicConsumeAsync(queue: queueDeclareOk.QueueName, autoAck: false, consumer: consumer);
        //Console.ReadKey();
    }
}
