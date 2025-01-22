using TemplateSimpleApi.Core.Repositories;

namespace TemplateSimpleApi.Core.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ProductRepository>();
        }
    }
}