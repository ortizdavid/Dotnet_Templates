using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace TemplateNatsApi.Common.Messaging.NATS;

public class NatsSubscriber : NatsClientBase
{
    private readonly ILogger<NatsSubscriber> _logger;

    public NatsSubscriber(IOptions<NatsSettings> settings, ILogger<NatsSubscriber> logger) : base(settings) 
    {
        _logger = logger;
    }

    public void Subscribe<T>(string subject)
    {
        EnsureConnection();
        Console.WriteLine($" [*] Subscribed to subject '{subject}' waiting to messages...");
        _logger.LogInformation($" [*] Subscribed to subject '{subject}' waiting to messages...");
        
        var subscription = _connection!.SubscribeAsync(subject);
        subscription.MessageHandler += (sender, args) =>
        {
            try
            {
                var messageData = Encoding.UTF8.GetString(args.Message.Data);
                var message = JsonSerializer.Deserialize<T>(messageData);
                Console.Write($"[x] Received message from subject '{subject}': {message}");
                _logger.LogInformation($"[x] Received message from subject '{subject}': {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error processing message from subject '{subject}': {ex.Message}");
                _logger.LogError($"[!] Error processing message from subject '{subject}': {ex.Message}");
                throw;
            }
        };

        subscription.Start();
    }

    public async Task ProcessMessageAsync<T>(string subject, Func<T, Task> funcProcessMessage)
    {
        EnsureConnection();
        Console.WriteLine($" [*] Subscribed to subject '{subject}' waiting to messages...");
        
        var subscription = _connection!.SubscribeAsync(subject);
        subscription.MessageHandler += async (sender, args) =>
        {
            try
            {
                var messageData = Encoding.UTF8.GetString(args.Message.Data);
                var message = JsonSerializer.Deserialize<T>(messageData);

                if (message is not null)
                {
                    await funcProcessMessage(message);
                    _logger.LogInformation($"[x] Processed message: {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[!] Error processing message from subject '{subject}': {ex.Message}");
                throw;
            }
        };
        
        subscription.Start();
        await Task.CompletedTask;
    }
}
