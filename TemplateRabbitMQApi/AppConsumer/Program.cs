using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TemplateRabbitMQApi.Common.Messaging.RabbitMQ;
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

        // Start Consuming Messages (Run as a Background Task)
        await rabbitConsumer.ConsumeFromQueue<MessageRequest>("dotnet_queue");
        Console.WriteLine(" [*] Consumers are running...");
        Console.ReadKey();

    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.AddSingleton<RabbitMQConsumer>();
    }
}
