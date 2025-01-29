using TemplateApi.Common.Messaging;

namespace TemplateApi.Common.Extensions;

public static class MessageExtensions
{
    public static void AddEmailConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the EmailSettings from appsettings.json
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // Register EmailService 
        services.AddSingleton<EmailService>();
    }
}