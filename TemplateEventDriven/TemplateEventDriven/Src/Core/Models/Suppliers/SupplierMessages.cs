namespace TemplateEventDriven.Core.Models.Suppliers;

public class SupplierCreated
{
    public Guid UniqueId { get; set; }
    public string? SupplierName { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? PrimaryPhone { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SupplierUpdated
{
    public Guid UniqueId { get; set; }
    public string? SupplierName { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? PrimaryPhone { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SupplierDeleted
{
    public Guid UniqueId { get; set; }
    public string? SupplierName { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? PrimaryPhone { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class SupplierImported
{
    public int TotalRecords { get; set; }
    public IEnumerable<Supplier>? Items { get; set; }
}