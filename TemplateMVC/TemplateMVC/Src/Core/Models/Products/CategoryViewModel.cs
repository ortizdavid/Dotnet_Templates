using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Products;

public class CategoryViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? CategoryName { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 10)]
    public string? Description { get; set; }
}
