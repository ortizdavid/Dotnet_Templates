using System.ComponentModel;

namespace TemplateMVC.Core.Models.Auth;

public class LoginViewModel
{
    [DisplayName("User Name")]
    public string? UserName { get; set; }

    [DisplayName("Password")]
    public string? Password { get; set; }
}
