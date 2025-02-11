namespace TemplateMVC.Common.Extensions;

public static class SessionExtensions
{
    public static void AddSessionConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("SessionSettings").Get<SessionSettings>();

        if (config is null || config.IdleTimeout <= 0)
        {
            throw new InvalidOperationException("SessionSettings configuration is missing in appsettings.json.");
        }

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(config.IdleTimeout);
            options.Cookie.MaxAge = TimeSpan.FromMinutes(config.CookieMaxAge);
            options.Cookie.HttpOnly = config.CookieHttpOnly;
            options.Cookie.IsEssential = config.CookieIsEssential;
        });
    }
}

public class SessionSettings
{
    public int IdleTimeout { get; set; } = 30;
    public int CookieMaxAge { get; set; } = 30;
    public bool CookieHttpOnly { get; set; } = true;
    public bool CookieIsEssential { get; set; } = true;
}