using Microsoft.EntityFrameworkCore;
using Serilog;
using Prometheus;
using TemplateSimpleApi.Models;
using TemplateSimpleApi.Repositories;
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
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add Repositories
        builder.Services.AddScoped<ProductRepository>();

        // Configure Serilog for logging with console output and Seq for centralized log management.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("C:/logs/dotnet-apps/dotnet-simple-api/app.log", rollingInterval: RollingInterval.Day)
            .WriteTo.GrafanaLoki("http://localhost:3100")
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Dotnet_Simple_API")
            .CreateLogger();

        builder.Host.UseSerilog();

        var app = builder.Build();

        // Use Prometheus metrics from /metrics endpoint
        app.UseMetricServer();
        app.UseHttpMetrics();

        // use Logger middleware
        app.UseSerilogRequestLogging();

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