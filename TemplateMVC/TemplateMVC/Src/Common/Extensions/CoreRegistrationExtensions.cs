using System.Reflection;
using TemplateMVC.Core.Services.Auth;

namespace TemplateMVC.Common.Extensions;

public static class CoreRegistrationExtensions
{
    // Register core repositories
    public static void AddCoreRepositories(this IServiceCollection services, Assembly assembly)
    {
        var types = GetCoreTypes("TemplateMVC.Core.Repositories", assembly);

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }

    // Register core services
    public static void AddCoreServices(this IServiceCollection services, Assembly assembly)
    {
        var types = GetCoreTypes("TemplateMVC.Core.Services", assembly);

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }

    // register Manual Core Services
    public static void AddCoreServicesManual(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    private static IEnumerable<Type> GetCoreTypes(string appNamespace, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null 
                && t.Namespace.StartsWith(appNamespace));
        return types;
    }
}