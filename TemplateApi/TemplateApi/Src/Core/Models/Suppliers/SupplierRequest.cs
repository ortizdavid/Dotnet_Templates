using System.ComponentModel.DataAnnotations;

namespace TemplateApi.Core.Services.Suppliers;

public class SupplierRequest
{
    [Required]
    [MaxLength(100)]
    public string? SupplierName { get; set; }

    [Required]
    [MaxLength(30)]
    public string? IdentificationNumber { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string? Email { get; set; }

    [Required]
    [Phone]
    [MaxLength(20)]
    public string? PrimaryPhone { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? SecondaryPhone { get; set; }

    [Required]
    public string? Address { get; set; }
}
