using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class GetRecoverLinkViewModel
{
    [Required]
    public string? Email { get; set; }
}