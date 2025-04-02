using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.Models.Auth;

public class GetRecoverLinkRequest
{
    [Required]
    public string? Email { get; set; }
}