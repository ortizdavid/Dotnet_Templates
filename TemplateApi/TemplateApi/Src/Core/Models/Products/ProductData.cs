namespace TemplateApi.Core.Models.Products
{
    public class ProductData
    {   
        public int ProductId { get; set; }
        public Guid UniqueId { get; set; }
        public string? ProductName { get; set; }
        public string? Code { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
    }
}
