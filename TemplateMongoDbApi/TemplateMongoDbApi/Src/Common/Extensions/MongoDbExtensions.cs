using MongoDB.Driver;

namespace TemplateMongoDbApi.Common.Extensions;

public static class MongoDbExtensions
{
    public static void AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Load settings from appsettings
        var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
        var mongoClient = new MongoClient(settings?.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings?.DatabaseName);

        services.AddSingleton<IMongoDatabase>(mongoDatabase);
    }
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
