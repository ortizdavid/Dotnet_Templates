namespace TemplateApi.Core.Models.Auth;

public class TokenResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}