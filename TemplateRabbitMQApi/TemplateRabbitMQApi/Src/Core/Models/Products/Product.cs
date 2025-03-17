using System.ComponentModel.DataAnnotations;
using TemplateRabbitMQApi.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using TemplateRabbitMQApi.Core.Models.Suppliers;

namespace TemplateRabbitMQApi.Core.Models.Products;

public class Product : IModel
{   
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string? ProductName { get; set; }

    [Required]
    [StringLength(30)]
    public string? Code { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [StringLength(150)]
    public string? Description { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int SupplierId { get; set; }

    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // RelationShips
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            // unique key
            entity.HasIndex(e => e.Code).IsUnique();

            entity.Property(p => p.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            // Foreign keys
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
