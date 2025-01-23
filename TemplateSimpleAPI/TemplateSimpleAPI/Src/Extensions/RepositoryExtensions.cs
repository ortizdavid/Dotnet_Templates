using TemplateSimpleApi.Repositories;

namespace TemplateSimpleApi.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ProductRepository>();
        }
    }
}