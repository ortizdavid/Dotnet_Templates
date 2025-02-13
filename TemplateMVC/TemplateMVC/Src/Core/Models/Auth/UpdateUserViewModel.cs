using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth
{
    public class UpdateUserViewModel
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
        
        public Guid UniqueId { get; set; }

        public string? RoleName { get; set; }

        public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
    }
}
