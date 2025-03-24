namespace TemplateEventDriven.Core.Models.Products;

public class ProductCreated
{
    public Guid UniqueId { get; set; }
    public string? ProductName { get; set; }
    public string? Code { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Supplier { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductUpdated
{
    public Guid UniqueId { get; set; }
    public string? ProductName { get; set; }
    public string? Code { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Supplier { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProductDeleted
{
    public Guid UniqueId { get; set; }
    public string? ProductName { get; set; }
    public string? Code { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Supplier { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class ProductImported
{
    public int TotalRecords { get; set; }
    public IEnumerable<Product>? Items { get; set; }
}

public class ProductCreatedImage
{
    public Guid UniqueId { get; set; }
    public string? ProductName { get; set; }
    public List<string?>? Images { get; set; }
    public string? UploadDir { get; set; }
    public DateTime CreatedAt { get; set; }
}