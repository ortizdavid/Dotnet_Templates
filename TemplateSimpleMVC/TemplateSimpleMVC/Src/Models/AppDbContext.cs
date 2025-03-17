using Microsoft.EntityFrameworkCore;

namespace TemplateSimpleMVC.Models;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        User.ConfigureModel(modelBuilder); 
        Product.ConfigureModel(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}


