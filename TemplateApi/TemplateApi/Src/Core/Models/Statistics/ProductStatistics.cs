namespace TemplateApi.Core.Models.Statistics;

public class ProductTotalPriceByCategories
{
    public string? CategoryName { get; set; }
    public decimal TotalPrice { get; set; }
}

public class ProductTotalPriceBySuppliers
{
    public string? SupplierName { get; set; }
    public decimal? TotalPrice { get; set; }
}
