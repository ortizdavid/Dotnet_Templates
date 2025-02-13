using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class ChangePasswordViewModel
{
    [Required]
    [StringLength(250)]
    [DisplayName("New Password")]
    public string? NewPassword { get; set; }

    [Required]
    [StringLength(250)]
    [DisplayName("Password Confirmation")]
    public string? PasswordConfirmation { get; set; }
}
