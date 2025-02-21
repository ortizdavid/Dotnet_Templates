namespace TemplateMVC.Core.Controllers.Reports;

public class ReportPath
{
    public static string GetTemplate(string reportPath, string fileName)
    {
        return $"~/Views/{reportPath}/{fileName}/Index.cshtml";
    }
}