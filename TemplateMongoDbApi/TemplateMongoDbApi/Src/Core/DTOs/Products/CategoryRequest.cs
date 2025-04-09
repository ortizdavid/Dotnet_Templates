using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.DTOs.Products;

public class CategoryRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? CategoryName { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string? Description { get; set; }
}
