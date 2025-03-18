using TemplateEventDriven.Common.Messaging.Email;
using TemplateEventDriven.Common.Messaging.RabbitMQ;

namespace TemplateEventDriven.Common.Extensions;

public static class MessagingExtensions
{
    public static void AddRabbitMQConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.AddSingleton<RabbitMQProducer>();
        services.AddSingleton<RabbitMQConsumer>();
    }

    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddSingleton<EmailService>();
    }
}