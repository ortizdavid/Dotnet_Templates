using Microsoft.EntityFrameworkCore;

namespace TemplateMinimalAPI.Models;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}