namespace TemplateMVC.Core.Models.Reports;

public class ProductReport
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? Code { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
}
