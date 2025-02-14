using System.ComponentModel;

namespace TemplateMVC.Core.Models.Auth;

public class DectivateUserViewModel
{
    public Guid UniqueId { get; set; }

    [DisplayName("User Name")]
    public string? UserName { get; set; }

    [DisplayName("Role")]
    public string? Role { get; set; }
}