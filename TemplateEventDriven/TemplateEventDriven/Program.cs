using TemplateEventDriven.Common.Extensions;
using TemplateEventDriven.Common.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Default Framework Services
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

        // Add services to the container.
        builder.Services.AddOpenApi();

        // Add Custom Services
        builder.Host.AddSerilogConfiguration();
        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddRabbitMQConfiguration(configuration);
        builder.Services.AddEmailConfigurations(configuration);
        builder.Services.AddDatabaseConfiguration(configuration);
        builder.Services.AddRepositories();
        builder.Services.AddServices();

        var app = builder.Build();

        app.LogApplicationStartup();

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