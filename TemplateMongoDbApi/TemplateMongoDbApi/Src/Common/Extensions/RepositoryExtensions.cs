using TemplateMongoDbApi.Core.Repositories;
using TemplateMongoDbApi.Core.Repositories.Auth;
using TemplateMongoDbApi.Core.Repositories.Products;
using TemplateMongoDbApi.Core.Repositories.Reports;
using TemplateMongoDbApi.Core.Repositories.Statistics;
using TemplateMongoDbApi.Core.Repositories.Suppliers;

namespace TemplateMongoDbApi.Common.Extensions;

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
