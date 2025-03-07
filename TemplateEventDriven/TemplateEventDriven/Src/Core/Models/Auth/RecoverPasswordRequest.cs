using System.ComponentModel.DataAnnotations;

namespace TemplateEventDriven.Core.Models.Auth;

public class GetRecoverLinkRequest
{
    [Required]
    public string? Email { get; set; }
}