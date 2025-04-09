namespace TemplateMongoDbApi.Core.DTOs.Products;

public class ProductImageResponse
{
    public string? ImageId { get; set; }
    public string? FileName { get; set; }
    public string? UploadDir { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}