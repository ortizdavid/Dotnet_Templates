using TemplateNatsApi.Core.Repositories;
using TemplateNatsApi.Core.Repositories.Auth;
using TemplateNatsApi.Core.Repositories.Products;
using TemplateNatsApi.Core.Repositories.Reports;
using TemplateNatsApi.Core.Repositories.Statistics;
using TemplateNatsApi.Core.Repositories.Suppliers;

namespace TemplateNatsApi.Common.Extensions;

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
