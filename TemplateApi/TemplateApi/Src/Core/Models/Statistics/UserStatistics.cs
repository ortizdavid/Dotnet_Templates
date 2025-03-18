namespace TemplateApi.Core.Models.Statistics;

public class UserActivesAndInactives
{
    public int ActiveUsers { get; set; }
    public int InactiveUsers { get; set; }
}

public class UserPercentageActivesAndInactives
{
    public decimal ActivePercentage { get; set; }
    public decimal InactivePercentage { get; set; }
}

