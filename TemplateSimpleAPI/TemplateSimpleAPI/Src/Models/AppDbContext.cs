using Microsoft.EntityFrameworkCore;

namespace TemplateSimpleApi.Models
{
    public class AppDbContext : DbContext
    {
        public required DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            Product.ConfigureModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}