using TemplateRabbitMQApi.Common.Extensions;

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
        builder.Services.AddRabbitMQConfiguration(configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}