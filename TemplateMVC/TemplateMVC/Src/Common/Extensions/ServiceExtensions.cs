using TemplateMVC.Core.Services.Auth;
using TemplateMVC.Core.Services.Products;
using TemplateMVC.Core.Services.Reports;
using TemplateMVC.Core.Services.Statistics;
using TemplateMVC.Core.Services.Suppliers;

namespace TemplateMVC.Common.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        // Auth
        services.AddTransient<AuthService>();
        services.AddTransient<RoleService>();
        services.AddTransient<UserService>();

        // Products
        services.AddTransient<CategoryService>();
        services.AddTransient<SupplierService>();
        services.AddTransient<ProductService>();
        
        // Reports
        services.AddTransient<CategoryReportService>();
        services.AddTransient<SupplierReportService>();
        services.AddTransient<ProductReportService>();

        // Statistics
        services.AddTransient<UserStatisticsService>();
        services.AddTransient<CategoryStatisticsService>();
        services.AddTransient<SupplierStatisticsService>();
        services.AddTransient<ProductStatisticsService>();
    }
}