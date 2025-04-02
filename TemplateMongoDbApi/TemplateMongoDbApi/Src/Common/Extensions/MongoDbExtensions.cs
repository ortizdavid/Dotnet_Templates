using MongoDB.Driver;
using TemplateMongoDbApi.Core.Models;

namespace TemplateMongoDbApi.Common.Extensions;

public static class MongoDbExtensions
{
    public static void AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Load settings from appsettings
        var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings?.ConnectionString)); 

        services.AddScoped<IMongoDatabase>(sp => 
        {
            var client = sp.GetRequiredService<MongoClient>();
            return client.GetDatabase(settings?.DatabaseName);
        });
    }
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}