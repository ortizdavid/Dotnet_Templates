using System.ComponentModel.DataAnnotations;

namespace TemplateApi.Core.Models.Auth
{
    public class GetRecoverLinkRequest
    {
        [Required]
        public string? Email { get; set; }
    }
}