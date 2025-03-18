using System.ComponentModel;

namespace TemplateMVC.Core.Models.Products;

public class DeleteCategoryViewModel
{
    public Guid UniqueId { get; set; }

    [DisplayName("Category Name")]
    public string? CategoryName { get; set; }
}