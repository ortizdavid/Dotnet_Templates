using TemplateApi.Core.Repositories;
using TemplateApi.Core.Repositories.Auth;
using TemplateApi.Core.Repositories.Products;
using TemplateApi.Core.Repositories.Reports;
using TemplateApi.Core.Repositories.Statistics;
using TemplateApi.Core.Repositories.Suppliers;

namespace TemplateApi.Common.Extensions;

public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<RoleRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<UserRefreshTokenRepository>();
        
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
