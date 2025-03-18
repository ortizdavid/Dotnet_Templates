using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TemplateEventDriven.Core.Models.Auth;
using TemplateEventDriven.Core.Models.Events;
using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Models.Suppliers;

namespace TemplateEventDriven.Core.Models;

public class AppDbContext : DbContext
{
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Supplier> Suppliers { get; set; }
    public required DbSet<Product> Products { get; set; }
    public required DbSet<ProductImage> ProductImages { get; set; }
    public required DbSet<EventType> EventTypes { get; set; }
    public required DbSet<UserEvent> UserEvents { get; set; }
    public required DbSet<ProductEvent> ProductEvents { get; set; }
    public required DbSet<CategoryEvent> CategoryEvents { get; set; }
    public required DbSet<SupplierEvent> SupplierEvents { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)  
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Get all types implementing IModel
        var modelTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IModel).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        // Call ConfigureModel on each model
        foreach (var modelType in modelTypes)
        {
            var method = modelType.GetMethod(nameof(IModel.ConfigureModel));
            method?.Invoke(null, new object[] { modelBuilder });
        }
    }
}
