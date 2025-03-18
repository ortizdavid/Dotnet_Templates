using System.ComponentModel;

namespace TemplateMVC.Core.Models.Reports;

public class ReportFilter
{
    [DisplayName("Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1).Date;

    [DisplayName("End Date")]
    public DateTime EndDate { get; set; } = DateTime.Now.Date;

    [DisplayName("Format")]
    public string Format { get; set; } = string.Empty;

    public bool HasValidDateRange => StartDate <= EndDate;
}