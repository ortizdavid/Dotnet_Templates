using System.ComponentModel;

namespace TemplateMVC.Core.Models.Products;

public class UploadProductImageViewModel
{
    [DisplayName("Product Name")]
    public string? ProductName { get; set; }

    public Guid UniqueId { get; set; }
}