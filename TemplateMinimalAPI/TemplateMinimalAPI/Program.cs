using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using TemplateMinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOpenApi();

// configure database
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Configure Serilog for logging with console output and Seq for centralized log management.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("C:/logs/dotnet-apps/dotnet-minimal-api/app.log", rollingInterval: RollingInterval.Day)
    .WriteTo.GrafanaLoki("http://localhost:3100")
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Dotnet_Minimal_API")
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Use Prometheus metrics from /metrics endpoint
app.UseMetricServer();
app.UseHttpMetrics();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Root
app.MapGet("/", () => "Template Minimal API");
app.MapGet("/api", () => "Template Minimal API");

// Get All
app.MapGet("/api/products", async (AppDbContext db) =>
{
    var products = await db.Products.ToListAsync();
    return Results.Ok(products);
});

// Get by Id
app.MapGet("/api/products/{id}", async (AppDbContext db, int id) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound($"Product with ID '{id}' not found");
    }
    return Results.Ok(product);
});

// Create
app.MapPost("/api/products", async (AppDbContext db, Product product) =>
{
    if (product is null)
    {
        return Results.BadRequest("Product request cannot be null");
    }
    var exists = await db.Products.AnyAsync(p => p.Code == product.Code);
    if (exists)
    {
        return Results.Conflict($"Product with code '{product.Code}' already exists");
    }
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

// Update
app.MapPut("/api/products/{id}", async (AppDbContext db, int id, Product productReq) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound($"Product with ID '{id}' not found");
    }
    product.Name = productReq.Name;
    product.Code = productReq.Code;
    product.Price = productReq.Price;
    db.Products.Update(product);
    await db.SaveChangesAsync();
    return Results.Ok(product);
});

// Delete
app.MapDelete("/api/products/{id}", async (AppDbContext db, int id) => 
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound($"Product with ID '{id}' not found");
    }
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

