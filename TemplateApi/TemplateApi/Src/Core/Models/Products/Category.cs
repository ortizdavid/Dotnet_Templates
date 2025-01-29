using System.ComponentModel.DataAnnotations;
using TemplateApi.Helpers;

namespace TemplateApi.Core.Models.Products;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
