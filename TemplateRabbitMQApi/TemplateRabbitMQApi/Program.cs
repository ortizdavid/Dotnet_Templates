using System.Reflection;
using Prometheus;
using TemplateRabbitMQApi.Common.Extensions;
using TemplateRabbitMQApi.Common.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        // Default Framework Services
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

        // Add services to the container.
        builder.Services.AddOpenApi();

        // Add Custom Services
        builder.Host.AddSerilogConfiguration();
        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddRabbitMQConfiguration(configuration);
        builder.Services.AddEmailConfiguration(configuration);
        builder.Services.AddDatabaseConfiguration(configuration);
        builder.Services.AddCoreRepositories(Assembly.GetExecutingAssembly());;
        builder.Services.AddCoreServices(Assembly.GetExecutingAssembly());;

        var app = builder.Build();

        app.LogApplicationStartup();

        // Use Prometheus metrics from /metrics endpoint
        app.UseMetricServer();
        app.UseHttpMetrics();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Use custom middlewares
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}