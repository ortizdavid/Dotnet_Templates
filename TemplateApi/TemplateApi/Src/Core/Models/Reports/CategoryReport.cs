namespace TemplateApi.Core.Models.Reports;

public class CategoryReport
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}