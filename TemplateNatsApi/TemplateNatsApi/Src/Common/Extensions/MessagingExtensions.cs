using TemplateNatsApi.Common.Messaging.Email;
using TemplateNatsApi.Common.Messaging.NATS;

namespace TemplateNatsApi.Common.Extensions;

public static class MessagingExtensions
{
    public static void AddNatsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<NatsSettings>(configuration.GetSection("NatsSettings"));
        services.AddSingleton<NatsPublisher>();
        services.AddSingleton<NatsSubscriber>();
    }

    public static void AddEmailConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddSingleton<EmailService>();
    }
}