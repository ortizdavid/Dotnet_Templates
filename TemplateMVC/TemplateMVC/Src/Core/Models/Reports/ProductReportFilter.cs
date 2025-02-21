using System.ComponentModel;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Models.Suppliers;

namespace TemplateMVC.Core.Models.Reports;

public class ProductReportFilter : ReportFilter
{
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    [DisplayName("Supplier")]
    public int SupplierId { get; set; }

    public IEnumerable<Category> Categories { get; set; } = [];
    
    public IEnumerable<Supplier> Suppliers { get; set; } = [];
}