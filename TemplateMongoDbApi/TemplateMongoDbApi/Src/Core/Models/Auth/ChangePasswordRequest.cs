using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.Models.Auth;

public class ChangePasswordRequest
{
    [Required]
    [StringLength(250)]
    public string? NewPassword { get; set; }

    [Required]
    [StringLength(250)]
    public string? PasswordConfirmation { get; set; }
}
