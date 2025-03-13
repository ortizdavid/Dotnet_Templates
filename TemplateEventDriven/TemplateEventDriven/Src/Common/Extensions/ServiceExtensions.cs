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
        services.AddScoped<UserCommandService>();
        services.AddScoped<UserQueryService>();

        // Products
        services.AddScoped<CategoryCommandService>();
        services.AddScoped<CategoryQueryService>();
        services.AddScoped<ProductCommandService>();
        services.AddScoped<ProductQueryService>();
        services.AddScoped<SupplierCommandService>();
        services.AddScoped<SupplierQueryService>();
        
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