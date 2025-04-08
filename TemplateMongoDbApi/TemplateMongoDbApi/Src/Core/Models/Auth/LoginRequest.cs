using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.Models.Auth;

public class LoginRequest
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
}
