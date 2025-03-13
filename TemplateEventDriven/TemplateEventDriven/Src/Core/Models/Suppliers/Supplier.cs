using System.ComponentModel.DataAnnotations;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;
using TemplateEventDriven.Common.Helpers;

namespace TemplateEventDriven.Core.Models.Suppliers;

[EFIndex(nameof(IdentificationNumber), IsUnique = true)]
[EFIndex(nameof(Email), IsUnique = true)]
[EFIndex(nameof(PrimaryPhone), IsUnique = true)]
[EFIndex(nameof(SecondaryPhone), IsUnique = true)]
public class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [Required]
    [StringLength(150)]
    public string? SupplierName { get; set; }

    [Required]
    [StringLength(30)]
    public string? IdentificationNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string? Email { get; set; }  

    [Required]
    [StringLength(20)]
    public string? PrimaryPhone { get; set; } 

    [StringLength(20)]
    public string? SecondaryPhone { get; set; } 

    [StringLength(150)]
    public string? Address { get; set; }

    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
}
