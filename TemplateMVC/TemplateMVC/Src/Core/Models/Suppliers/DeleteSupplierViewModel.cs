using System.ComponentModel;

namespace TemplateMVC.Core.Models.Suppliers;

public class DeleteSupplierViewModel
{
    public Guid UniqueId { get; set; }

    [DisplayName("Supplier Name")]
    public string? SupplierName { get; set; }

    [DisplayName("Identification Number")]
    public string? IdentificationNumber { get; set; }
}