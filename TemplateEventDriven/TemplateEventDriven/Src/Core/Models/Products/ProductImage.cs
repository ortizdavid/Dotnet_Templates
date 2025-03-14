using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TemplateEventDriven.Core.Models.Products;

public class ProductImage : IModel
{   
    [Key]
    public int ImageId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [StringLength(150)]
    public string? FileName { get; set; }

    [StringLength(150)]
    public string? UploadDir { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // RelationShips
    public Product? Product { get; set; }

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
