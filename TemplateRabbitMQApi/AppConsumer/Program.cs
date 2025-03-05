using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TemplateRabbitMQApi.Common.Messaging.RabbitMQ;
using TemplateRabbitMQApi.Core.Models.Messaging;
using AppConsumer.Models;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
       var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Setup Dependency Injection
        var services = new ServiceCollection();
        ConfigureServices(services, configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Resolve the RabbitMQ Consumer
        var rabbitConsumer = serviceProvider.GetRequiredService<RabbitMQConsumer>();

        // Start Consuming Messages 

        // queue: dotnet_queue
        //await ConsumeDotnetQueue(rabbitConsumer);

        // exchage: Category Created
        //await ConsumeCategoriesCreated(rabbitConsumer);
        await ConsumeCategoriesUpdated(rabbitConsumer);
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.AddSingleton<RabbitMQConsumer>();
    }

    public static async Task ConsumeDotnetQueue(RabbitMQConsumer consumer)
    {
        try
        {
            await consumer.ConsumeFromQueue<MessageRequest>("dotnet_queue");
            Console.WriteLine(" [*] Consumers are running...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Consumer error: {ex.Message}");
        }
    }

    public static async Task ConsumeCategoriesCreated(RabbitMQConsumer consumer)
    {
        try
        {
            await consumer.ConsumeFromExchange<Category>(Exchanges.CategoryExchange, RoutingKeys.Category.Created);
            Console.WriteLine(" [*] Consumers are running...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Consumer error: {ex.Message}");
        }
    }

    public static async Task ConsumeCategoriesUpdated(RabbitMQConsumer consumer)
    {
        try
        {
            await consumer.ConsumeFromExchange<Category>(Exchanges.CategoryExchange, RoutingKeys.Category.Updated);
            Console.WriteLine(" [*] Consumers are running...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Consumer error: {ex.Message}");
        }
    }
}
