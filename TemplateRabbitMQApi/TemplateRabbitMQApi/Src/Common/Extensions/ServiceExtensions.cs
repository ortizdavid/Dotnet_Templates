using TemplateRabbitMQApi.Core.Services.Auth;
using TemplateRabbitMQApi.Core.Services.Products;
using TemplateRabbitMQApi.Core.Services.Reports;
using TemplateRabbitMQApi.Core.Services.Statistics;
using TemplateRabbitMQApi.Core.Services.Suppliers;

namespace TemplateRabbitMQApi.Common.Extensions;

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
    }
}