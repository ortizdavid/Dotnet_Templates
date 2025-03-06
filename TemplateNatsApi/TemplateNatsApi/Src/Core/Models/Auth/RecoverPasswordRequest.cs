using System.ComponentModel.DataAnnotations;

namespace TemplateNatsApi.Core.Models.Auth;

public class GetRecoverLinkRequest
{
    [Required]
    public string? Email { get; set; }
}