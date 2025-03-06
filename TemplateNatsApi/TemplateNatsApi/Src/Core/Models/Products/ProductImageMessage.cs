using System.ComponentModel.DataAnnotations;

namespace TemplateNatsApi.Core.Models.Products;

public class ProductImageMessage
{   
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? FileName { get; set; }
    public string? UploadDir { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
