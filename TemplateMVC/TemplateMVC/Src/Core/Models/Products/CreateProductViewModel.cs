using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TemplateMVC.Core.Models.Suppliers;

namespace TemplateMVC.Core.Models.Products;

public class CreateProductViewModel
{
    [Required]
    [DisplayName("Product Name")]
    [StringLength(50, MinimumLength = 3)]
    public string? ProductName { get; set; }
    
    [Required]
    [DisplayName("Code")]
    [StringLength(20, MinimumLength = 3)]
    public string? Code { get; set; }

    [Required]
    [DisplayName("Unit Price")]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [StringLength(100, MinimumLength = 3)]
    [DisplayName("Description")]
    public string? Description { get; set; }
    
    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    [Required]
    [DisplayName("Supplier")]
    public int SupplierId { get; set; }

    public Guid UniqueId { get; set; }

    public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
    
    public IEnumerable<Supplier> Suppliers { get; set; } = Enumerable.Empty<Supplier>();
}
