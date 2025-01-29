namespace TemplateApi.Core.Models.Suppliers;

public class SupplierReport
{
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Email { get; set; }  
    public string? PrimaryPhone { get; set; } 
    public string? SecondaryPhone { get; set; } 
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}
