namespace TemplateNatsApi.Core.Models.Products;

public class ProductMessage
{
    public Guid UniqueId { get; set; }
    public string? ProductName { get; set; }
    public string? Code { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Category { get; set; }
    public string? Supplier { get; set; }
}