namespace TemplateMVC.Core.Controllers.Statistics;

public class StatisticsViewPath
{
    public static string Get(string reportPath, string fileName)
    {
        return $"~/Views/Statistics/{reportPath}/{fileName}.cshtml";
    }
}