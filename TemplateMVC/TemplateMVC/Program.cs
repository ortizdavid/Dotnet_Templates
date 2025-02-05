using TemplateMVC.Common.Extensions;
using TemplateMVC.Common.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        // Default Framework Services
        builder.Services.AddSession();
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();

        // Application-Specific Services via Extensions
        builder.Host.AddSerilogConfiguration();
        builder.Services.AddEmailConfigurations(configuration);
        builder.Services.AddDatabaseConfiguration(configuration);
        builder.Services.AddRepositories();
        builder.Services.AddServices();

        var app = builder.Build();

        app.LogApplicationStartup();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        // Session
        app.UseSession();

        // Custom Middlewares
        app.UseMiddleware<AuthMiddleware>();
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();


        app.Run();
    }
}