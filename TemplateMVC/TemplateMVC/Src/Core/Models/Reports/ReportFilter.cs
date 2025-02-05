namespace TemplateMVC.Core.Models.Reports;

public class ReportFilter
{
    public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1).Date;
    public DateTime EndDate { get; set; } = DateTime.Now.Date;
    public string Format { get; set; } = string.Empty;
}