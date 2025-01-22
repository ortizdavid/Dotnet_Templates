using Microsoft.EntityFrameworkCore;

namespace TemplateSimpleApi.Core.Models
{
    public class AppDbContext : DbContext
    {
        public required DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("DECIMAL(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}