using System.ComponentModel.DataAnnotations;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;
using TemplateEventDriven.Common.Helpers;

namespace TemplateEventDriven.Core.Models.Products;

[EFIndex(nameof(CategoryName), IsUnique = true)]
public class Category
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
}
