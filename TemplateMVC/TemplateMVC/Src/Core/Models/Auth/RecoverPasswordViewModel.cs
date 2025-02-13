using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class GetRecoverLinkViewModel
{
    [Required]
    [DisplayName("Email")]
    public string? Email { get; set; }
}