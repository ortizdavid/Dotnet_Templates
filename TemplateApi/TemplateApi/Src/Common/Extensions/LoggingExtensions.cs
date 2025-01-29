using Serilog;

namespace TemplateApi.Common.Extensions;

public static class LoggingExtensions
{
    public static void AddSerilogConfiguration(this IHostBuilder hostBuilder)
    {
        // Use IConfigurationBuilder to add the serilog.json file
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  
            .AddJsonFile("serilog.json", optional: false, reloadOnChange: true);  

        // Read the loaded configuration into the application configuration
        var finalConfig = configBuilder.Build();

        // Configure Serilog using the final configuration
        hostBuilder.UseSerilog((context, services, configuration) => 
            configuration.ReadFrom.Configuration(finalConfig)  
        );
    }

    public static void LogApplicationStartup(this WebApplication app)
    {
        var hostUrl = app.Configuration["Kestrel:Endpoints:Http:Url"];
        
        if (!string.IsNullOrEmpty(hostUrl))
        {
            app.Logger.LogInformation($"Application is running at {hostUrl}");
        }
        else
        {
            app.Logger.LogWarning("Application URL not found in configuration.");
        }
    }
}