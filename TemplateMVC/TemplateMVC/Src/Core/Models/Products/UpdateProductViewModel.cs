using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Products;

public class UpdateProductViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? ProductName { get; set; }
    
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string? Code { get; set; }

    [Required]
    [Range(0, 1_000_000)]
    public decimal UnitPrice { get; set; }

    [StringLength(100, MinimumLength = 3)]
    public string? Description { get; set; }
    
    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int SupplierId { get; set; }
}
