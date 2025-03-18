using TemplateRabbitMQApi.Core.Repositories;
using TemplateRabbitMQApi.Core.Repositories.Auth;
using TemplateRabbitMQApi.Core.Repositories.Products;
using TemplateRabbitMQApi.Core.Repositories.Reports;
using TemplateRabbitMQApi.Core.Repositories.Statistics;
using TemplateRabbitMQApi.Core.Repositories.Suppliers;

namespace TemplateRabbitMQApi.Common.Extensions;

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
