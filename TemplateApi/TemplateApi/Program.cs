using TemplateApi.Common.Extensions;
using TemplateApi.Common.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

          // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Default Framework Services
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

      

        // Application-Specific Services via Extensions
        builder.Host.AddSerilogConfiguration();
        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddEmailConfigurations(configuration);
        builder.Services.AddDatabaseConfiguration(configuration);
        builder.Services.AddRepositories();
        builder.Services.AddServices();

        // Application Initialization
        var app = builder.Build();

        app.LogApplicationStartup();

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
