using System.Reflection;

namespace TemplateRabbitMQApi.Common.Extensions;

public static class CoreRegistrationExtensions
{

    // Register core repositories
    public static void AddCoreRepositories(this IServiceCollection services, Assembly assembly)
    {
        var types = GetCoreTypes("TemplateRabbitMQApi.Core.Repositories", assembly);

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }

    // Register core services
    public static void AddCoreServices(this IServiceCollection services, Assembly assembly)
    {
        var types = GetCoreTypes("TemplateRabbitMQApi.Core.Services", assembly);

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }

    private static IEnumerable<Type> GetCoreTypes(string appNamespace, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null 
                && t.Namespace.StartsWith(appNamespace));
        return types;
    }
}