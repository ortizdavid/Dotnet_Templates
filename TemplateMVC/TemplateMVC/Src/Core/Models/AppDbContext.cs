using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Models.Suppliers;

namespace TemplateMVC.Core.Models;

public class AppDbContext : DbContext
{
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Supplier> Suppliers { get; set; }
    public required DbSet<Product> Products { get; set; }
    public required DbSet<ProductImage> ProductImages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)  
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.UnitPrice)
            .HasColumnType("decimal(18, 2)");

        base.OnModelCreating(modelBuilder);
    }
}
