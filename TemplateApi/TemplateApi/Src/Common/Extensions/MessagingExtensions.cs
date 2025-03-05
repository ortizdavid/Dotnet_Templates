using TemplateApi.Common.Messaging;

namespace TemplateApi.Common.Extensions;

public static class MessagingExtensions
{
    public static void AddEmailConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddSingleton<EmailService>();
    }
}