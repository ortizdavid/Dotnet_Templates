using Microsoft.EntityFrameworkCore;
using TemplateMinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();

// configure database
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Test
app.MapGet("/", () => "Template for Minimal API");
app.MapGet("/api", () => "Template for Minimal API");

// Get All
app.MapGet("/api/products", async (AppDbContext db) =>
{
    var products = await db.Products.ToListAsync();
    return Results.Ok(products);
});

// Get by Id
app.MapGet("/api/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound($"Product with ID '{id}' not found");
    }
    return Results.Ok(product);
});

// Create
app.MapPost("/api/products", async (Product product, AppDbContext db) =>
{
    if (product is null)
    {
        return Results.BadRequest("Product cannot be null");
    }
    var exists = db.Products.Any(p => p.Code == product.Code);
    if (exists)
    {
        return Results.Conflict($"Product with code '{product.Code}' already exists");
    }
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

// Update
app.MapPut("/api/products/{id}", async (Product productReq, int id, AppDbContext db) =>
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
app.MapDelete("/api/products/{id}", async (int id, AppDbContext db) => 
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

