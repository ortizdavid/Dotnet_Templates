using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateEventDriven.Core.Models.Products;

public class ProductImage
{   
    [Key]
    public int ImageId { get; set; }

    [Required]
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }

    [StringLength(150)]
    public string? FileName { get; set; }

    [StringLength(150)]
    public string? UploadDir { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
