using TemplateMongoDbApi.Core.Models.Suppliers;

namespace TemplateMongoDbApi.Core.Models.Products;

public class ProductResponse
{   
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? Code { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }
    public CategoryResponse? Category { get; set; }
    public SupplierResponse? Supplier { get; set; }
}
