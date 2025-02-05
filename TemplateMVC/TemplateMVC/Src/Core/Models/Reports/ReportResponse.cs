namespace TemplateMVC.Core.Models.Reports;

public class ReportResponse <T> where T : class
{
    public Metadata? Metadata { get; set; }
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
}

public class Metadata
{
    public int TotalRecords { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
}