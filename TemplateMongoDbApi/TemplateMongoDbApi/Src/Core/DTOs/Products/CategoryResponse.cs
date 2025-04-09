namespace TemplateMongoDbApi.Core.DTOs.Products;

public class CategoryResponse 
{
    public string? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}