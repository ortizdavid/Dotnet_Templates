using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth
{
    public class CreateUserViewModel
    {
        [Required]
        [DisplayName("Role")]
        public int RoleId { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("User Name")]
        public string? UserName { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Email")]
        public string? Email { get; set; }

        
        [Required]
        [StringLength(250)]
        [DisplayName("Password")]
        public string? Password { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Password Confirmation")]
        public string? PasswordConfirmation { get; set; }
        
        public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
    }
}
