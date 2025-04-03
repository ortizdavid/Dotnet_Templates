using TemplateRedisApi.Common.Messaging;

namespace TemplateRedisApi.Common.Extensions;

public static class MessagingExtensions
{
    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddSingleton<EmailService>();
    }
}