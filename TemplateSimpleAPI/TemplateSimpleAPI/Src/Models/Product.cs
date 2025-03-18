using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TemplateSimpleApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        
        [Required]
        [StringLength(20)]
        public string? Code { get; set; }

        [Required]
        public decimal Price { get; set; }

        public static void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(p => p.Code)
                    .IsUnique();

                entity.Property(p => p.Price)
                    .HasColumnType("DECIMAL(18,2)");
            });
        }
    }
}