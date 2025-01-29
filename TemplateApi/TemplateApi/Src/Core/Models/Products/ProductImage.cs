using System.ComponentModel.DataAnnotations;

namespace TemplateApi.Core.Models.Products;

public class ProductImage
{   
    [Key]
    public int ImageId { get; set; }
    public int ProductId { get; set; }
    public string? FileName { get; set; }
    public string? UploadDir { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
