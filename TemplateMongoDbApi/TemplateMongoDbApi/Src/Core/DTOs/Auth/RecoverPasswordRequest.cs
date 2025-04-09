using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.DTOs.Auth;

public class GetRecoverLinkRequest
{
    [Required]
    public string? Email { get; set; }
}