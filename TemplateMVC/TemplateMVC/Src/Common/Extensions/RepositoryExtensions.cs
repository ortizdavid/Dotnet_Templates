using TemplateMVC.Core.Repositories;
using TemplateMVC.Core.Repositories.Auth;
using TemplateMVC.Core.Repositories.Products;
using TemplateMVC.Core.Repositories.Reports;
using TemplateMVC.Core.Repositories.Statistics;
using TemplateMVC.Core.Repositories.Suppliers;

namespace TemplateMVC.Common.Extensions;

public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<RoleRepository>();
        services.AddScoped<UserRepository>();
        
        // Products
        services.AddScoped<CategoryRepository>();
        services.AddScoped<SupplierRepository>();
        services.AddScoped<ProductRepository>();
        services.AddScoped<ProductImageRepository>();

        // Reports
        services.AddScoped<CategoryReportRepository>();
        services.AddScoped<SupplierReportRepository>();   
        services.AddScoped<ProductReportRepository>();

        // Statistics
        services.AddScoped<UserStatisticsRepository>();
        services.AddScoped<CategoryStatisticsRepository>();
        services.AddScoped<SupplierStatisticsRepository>();
        services.AddScoped<ProductStatisticsRepository>();
    }
}
