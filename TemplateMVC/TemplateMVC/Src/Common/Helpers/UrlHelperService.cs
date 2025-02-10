namespace TemplateMVC.Common.Helpers;

public class UrlHelperService
{
    private readonly IHttpContextAccessor _contextAcessor;
    private HttpContext? _httpContext => _contextAcessor?.HttpContext;

    public UrlHelperService(IHttpContextAccessor contextAccessor)
    {
        _contextAcessor = contextAccessor;
    }

    public string GetBaseUrl()
    {
        return $"{_httpContext?.Request.Scheme}://{_httpContext?.Request.Host.Value}";
    }

    public string GetFullUrl(string relativePath)
    {
        return $"{GetBaseUrl().TrimEnd('/')}/{relativePath.TrimStart('/')}";
    }

    public string GetPasswordRecoveryLink(string? token)
    {
        return $"{GetBaseUrl()}/auth/recover-password/{token}";
    }

    public string GetRequestFullUrl()
    {
        return $"{_httpContext?.Request.Scheme}://{_httpContext?.Request.Host}{_httpContext?.Request.Path}{_httpContext?.Request.QueryString}";
    }

    public string GetUserAgent()
    {
        return _httpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown";
    }

    public string GetClientIp()
    {
        return _httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    public string GetReferrerUrl()
    {
        return _httpContext?.Request.Headers["Referer"].ToString() ?? string.Empty;
    }
}