namespace TemplateMVC.Core.Controllers.Reports;

public class ReportViewPath
{
    public static string Get(string reportPath, string fileName)
    {
        return $"~/Views/Reports/{reportPath}/{fileName}.cshtml";
    }
}