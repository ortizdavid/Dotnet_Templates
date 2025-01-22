using TemplateApi.Core.Services.Auth;
using TemplateApi.Core.Services.Products;
using TemplateApi.Core.Services.Reports;
using TemplateApi.Core.Services.Statistics;
using TemplateApi.Core.Services.Suppliers;

namespace TemplateApi.Common.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            // Auth
            services.AddTransient<JwtService>();
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
            services.AddSingleton<SupplierStatisticsService>();
            services.AddTransient<ProductStatisticsService>();
        }
    }
}