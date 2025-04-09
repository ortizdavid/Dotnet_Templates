using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.DTOs.Auth;

public class LoginRequest
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
}
