using TemplateNatsApi.Core.Services.Auth;
using TemplateNatsApi.Core.Services.Products;
using TemplateNatsApi.Core.Services.Reports;
using TemplateNatsApi.Core.Services.Statistics;
using TemplateNatsApi.Core.Services.Suppliers;

namespace TemplateNatsApi.Common.Extensions;

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