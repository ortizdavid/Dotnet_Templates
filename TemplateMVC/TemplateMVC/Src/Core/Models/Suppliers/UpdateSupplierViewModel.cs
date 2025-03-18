using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Suppliers;

public class UpdateSupplierViewModel
{
    [Required]
    [MaxLength(100)]
    [DisplayName("Supplier Name")]
    public string? SupplierName { get; set; }

    [Required]
    [MaxLength(30)]
    [DisplayName("Identification Number")]
    public string? IdentificationNumber { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    [DisplayName("Email")]
    public string? Email { get; set; }

    [Required]
    [Phone]
    [MaxLength(20)]
    [DisplayName("Primary Phone")]
    public string? PrimaryPhone { get; set; }

    [Phone]
    [MaxLength(20)]
    [DisplayName("Secondary Phone")]
    public string? SecondaryPhone { get; set; }

    [Required]
    [DisplayName("Address")]
    public string? Address { get; set; }

    public Guid UniqueId { get; set; }
}
