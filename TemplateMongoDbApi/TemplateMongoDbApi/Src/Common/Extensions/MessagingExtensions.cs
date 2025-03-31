using TemplateMongoDbApi.Common.Messaging;

namespace TemplateMongoDbApi.Common.Extensions;

public static class MessagingExtensions
{
    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddSingleton<EmailService>();
    }
}