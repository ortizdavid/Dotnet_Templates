namespace TemplateApi.Core.Models.Reports
{
    public class ProductReport
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Code { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
