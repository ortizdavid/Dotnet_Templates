using TemplateEventDriven.Core.Repositories;
using TemplateEventDriven.Core.Repositories.Auth;
using TemplateEventDriven.Core.Repositories.Events;
using TemplateEventDriven.Core.Repositories.Products;
using TemplateEventDriven.Core.Repositories.Reports;
using TemplateEventDriven.Core.Repositories.Statistics;
using TemplateEventDriven.Core.Repositories.Suppliers;

namespace TemplateEventDriven.Common.Extensions;

public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // Generic Repository
        services.AddScoped(typeof(RepositoryBase<>));
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

        // Events
        services.AddScoped<UserEventRepository>();
        services.AddScoped<ProductEventRepository>();
        services.AddScoped<CategoryEventRepository>();
        services.AddScoped<SupplierEventRepository>();
    }
}
