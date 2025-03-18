using System.ComponentModel;

namespace TemplateMVC.Core.Models.Products;
public class DeleteProductViewModel
{
    public Guid UniqueId { get; set; }

    [DisplayName("Product Name")]
    public string? ProductName { get; set; }

    [DisplayName("Code")]
    public string? Code { get; set; }

    [DisplayName("Price")]
    public decimal UnitPrice { get; set; }
}