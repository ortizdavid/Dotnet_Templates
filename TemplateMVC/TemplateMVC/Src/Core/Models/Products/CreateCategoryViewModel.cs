using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Products;

public class CreateCategoryViewModel
{
    [Required]
    [DisplayName("Category Name")]
    [StringLength(50, MinimumLength = 3)]
    public string? CategoryName { get; set; }

    [Required]
    [DisplayName("Description")]
    [StringLength(150, MinimumLength = 10)]
    public string? Description { get; set; }
}
