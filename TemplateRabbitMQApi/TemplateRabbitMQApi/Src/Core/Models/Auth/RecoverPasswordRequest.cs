using System.ComponentModel.DataAnnotations;

namespace TemplateRabbitMQApi.Core.Models.Auth;

public class GetRecoverLinkRequest
{
    [Required]
    public string? Email { get; set; }
}