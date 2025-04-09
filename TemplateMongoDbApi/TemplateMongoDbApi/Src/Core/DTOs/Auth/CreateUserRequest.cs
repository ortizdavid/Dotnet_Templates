using System.ComponentModel.DataAnnotations;

namespace TemplateMongoDbApi.Core.DTOs.Auth
{
    public class CreateUserRequest
    {
        [Required]
        public string? RoleCode { get; set; }

        [Required]
        [StringLength(150)]
        public string? UserName { get; set; }
        
        [Required]
        [StringLength(250)]
        public string? Password { get; set; }

        [Required]
        [StringLength(150)]
        public string? Email { get; set; }
    }
}
