using Microsoft.Extensions.Options;
using NATS.Client;

namespace TemplateNatsApi.Common.Messaging.NATS;

public class NatsClientBase
{
    protected readonly NatsSettings _settings;
    protected IConnection? _connection;
    
    protected NatsClientBase(IOptions<NatsSettings> settings)
    {
        _settings = settings.Value;
    }

    protected void EnsureConnection()
    {
        if (_connection is null || _connection.IsClosed())
        {
            var factory = new ConnectionFactory();
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = _settings.Url;
            opts.User = _settings.User;
            opts.Password = _settings.Password;
            opts.Timeout = _settings.Timeout;
            opts.ReconnectWait = _settings.ReconnectWait;
            opts.MaxReconnect = _settings.MaxReconnects;

            _connection = factory.CreateConnection(opts);
        }
    }

    public void Close()
    {
        if (_connection is not null)
        {
            _connection.Close();
            _connection.Drain();
        }
    }
}