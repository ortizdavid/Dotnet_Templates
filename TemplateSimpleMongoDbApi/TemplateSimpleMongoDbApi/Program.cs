using Serilog;
using Prometheus;
using TemplateSimpleMongoDbApi.Models;
using TemplateSimpleMongoDbApi.Services;
using Serilog.Sinks.Grafana.Loki;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        // Add Database configuration
        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

        // Add Services
        builder.Services.AddScoped<ProductService>();

        // Configure Serilog for logging with console output and Seq for centralized log management.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("C:/logs/dotnet-apps/dotnet-simple-mongodb-api/app.log", rollingInterval: RollingInterval.Day)
            .WriteTo.GrafanaLoki("http://localhost:3100")
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Dotnet_Simple_MongoDb_API")
            .CreateLogger();

        builder.Host.UseSerilog();

        var app = builder.Build();

        // Use Prometheus metrics from /metrics endpoint
        app.UseMetricServer();
        app.UseHttpMetrics();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        //app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}