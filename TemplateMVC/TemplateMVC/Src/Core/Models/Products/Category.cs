using System.ComponentModel.DataAnnotations;
using TemplateMVC.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace TemplateMVC.Core.Models.Products;

public class Category : IModel
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string? CategoryName { get; set; }

    [StringLength(150)]
    public string? Description { get; set; }

    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Relationships
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
            // unique key
            entity.HasIndex(e => e.CategoryName).IsUnique()
        );
    }
}
