using System.ComponentModel.DataAnnotations;

namespace TemplateApi.Core.Models.Auth
{
    public class CreateUserRequest
    {
        [Required]
        public int RoleId { get; set; }

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
