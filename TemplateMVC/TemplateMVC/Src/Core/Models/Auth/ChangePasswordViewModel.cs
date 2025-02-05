using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class ChangePasswordViewModel
{
    [Required]
    [StringLength(250)]
    public string? NewPassword { get; set; }

    [Required]
    [StringLength(250)]
    public string? PasswordConfirmation { get; set; }
}
