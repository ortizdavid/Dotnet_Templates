using Microsoft.EntityFrameworkCore;
using TemplateSimpleMVC.Controllers;
using TemplateSimpleMVC.Middlewares;
using TemplateSimpleMVC.Models;
using TemplateSimpleMVC.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add sessions service
        builder.Services.AddSession();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Context Accessor
        builder.Services.AddHttpContextAccessor();

        // Dbconnection
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(connectionString)
        );

        // Add Repository
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<ProductRepository>();
        builder.Services.AddScoped<UserContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();


        app.Run();
    }
}