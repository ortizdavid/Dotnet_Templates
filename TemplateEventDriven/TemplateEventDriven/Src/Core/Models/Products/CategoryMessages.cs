namespace TemplateEventDriven.Core.Models.Products;

public class CategoryCreated
{
    public Guid UniqueId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryUpdated
{
    public Guid UniqueId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CategoryDeleted
{
    public Guid UniqueId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class CategoryImported
{
    public int TotalRecords { get; set; }
    public IEnumerable<Category>? Items { get; set; }
}