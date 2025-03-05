using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TemplateRabbitMQApi.Common.Messaging.RabbitMQ;

public class RabbitMQClientBase
{
    protected readonly RabbitMQSettings _settings;
    protected IConnection? _connection;
    protected IChannel? _channel;
    
    protected RabbitMQClientBase(IOptions<RabbitMQSettings> settings)
    {
        _settings = settings.Value;
    }

    protected async Task EnsureConnectionAsync()
    {
        if (_connection is null || !_connection.IsOpen)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }
    }

    protected void CheckChannel(IChannel? channel)
    {
        if (channel is null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initalizated.");
        }
    }

    protected async Task DeclareQueueAsync(string queueName)
    {
        CheckChannel(_channel);
        await _channel!.QueueDeclareAsync(
            queue: queueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );
    }

    protected async Task DeclareExchangeAsync(string exchangeName, ExchangeType exchangeType = ExchangeType.Topic)
    {
        CheckChannel(_channel);
        await _channel!.ExchangeDeclareAsync(
            exchange: exchangeName, 
            type: exchangeType.ToString().ToLower(),
            durable: true,
            autoDelete: true
        );
    }

    public async Task<QueueDeclareOk> DeclareAndBindQueueAsync(string exchangeName, string routingKey)
    {
        CheckChannel(_channel);
        var queueDeclareOk = await _channel!.QueueDeclareAsync(
            queue: string.Empty, 
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await _channel.QueueBindAsync(
            queue: queueDeclareOk.QueueName,
            exchange: exchangeName,
            routingKey: routingKey
        );

        return queueDeclareOk;
    }

    public async Task CloseAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
        }
        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }
    }

    public async Task DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
        }
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}