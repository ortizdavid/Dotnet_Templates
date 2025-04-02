using System.Reflection;
using Prometheus;
using TemplateMongoDbApi.Common.Extensions;
using TemplateMongoDbApi.Common.Middlewares;

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

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Application-Specific Services via Extensions
        builder.Host.AddSerilogConfiguration();
        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddEmailConfiguration(configuration);
        builder.Services.AddMongoDbConfiguration(configuration);
        builder.Services.AddCoreRepositories(Assembly.GetExecutingAssembly());
        builder.Services.AddCoreServices(Assembly.GetExecutingAssembly());

        // Application Initialization
        var app = builder.Build();

        app.LogApplicationStartup();
        
        // Use Prometheus metrics from /metrics endpoint
        app.UseMetricServer();
        app.UseHttpMetrics();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Use custom middlewares
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
