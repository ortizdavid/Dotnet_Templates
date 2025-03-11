using TemplateEventDriven.Core.Services.Auth;
using TemplateEventDriven.Core.Services.Events;
using TemplateEventDriven.Core.Services.Products;
using TemplateEventDriven.Core.Services.Reports;
using TemplateEventDriven.Core.Services.Statistics;
using TemplateEventDriven.Core.Services.Suppliers;

namespace TemplateEventDriven.Common.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<JwtService>();
        services.AddScoped<AuthService>();
        services.AddScoped<RoleService>();
        services.AddScoped<UserService>();

        // Products
        services.AddScoped<CategoryService>();
        services.AddScoped<SupplierService>();
        services.AddScoped<ProductService>();
        
        // Reports
        services.AddScoped<CategoryReportService>();
        services.AddScoped<SupplierReportService>();
        services.AddScoped<ProductReportService>();

        // Statistics
        services.AddScoped<UserStatisticsService>();
        services.AddScoped<CategoryStatisticsService>();
        services.AddScoped<SupplierStatisticsService>();
        services.AddScoped<ProductStatisticsService>();

        // Events
        services.AddScoped(typeof(EventService<>));
    }
}