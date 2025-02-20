namespace TemplateMVC.Core.Models.Products;

public class ProductDetailsViewModel
{
    public ProductData Product { get; set; } = new();
    public IEnumerable<ProductImage> Images { get; set; } = [];
}
