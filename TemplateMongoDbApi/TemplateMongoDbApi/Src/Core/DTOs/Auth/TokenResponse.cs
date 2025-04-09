namespace TemplateMongoDbApi.Core.DTOs.Auth;

public class TokenResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}