using System.ComponentModel.DataAnnotations;
using TemplateNatsApi.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using TemplateNatsApi.Core.Models.Products;

namespace TemplateNatsApi.Core.Models.Suppliers;

public class Supplier : IModel
{
    [Key]
    public int SupplierId { get; set; }

    [Required]
    [StringLength(150)]
    public string? SupplierName { get; set; }

    [Required]
    [StringLength(30)]
    public string? IdentificationNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string? Email { get; set; }  

    [Required]
    [StringLength(20)]
    public string? PrimaryPhone { get; set; } 

    [StringLength(20)]
    public string? SecondaryPhone { get; set; } 

    [StringLength(150)]
    public string? Address { get; set; }

    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;

    // Relationships
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>(entity =>
        {
            // unique keys
            entity.HasIndex(e => e.IdentificationNumber).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.PrimaryPhone).IsUnique();
            entity.HasIndex(e => e.SecondaryPhone).IsUnique();
        });
    }
}