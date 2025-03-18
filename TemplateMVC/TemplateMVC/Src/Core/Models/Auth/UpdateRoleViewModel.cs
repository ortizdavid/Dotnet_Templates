using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class UpdateRoleViewModel
{
    public Guid UniqueId { get; set; }
    
    [Required]
    [StringLength(150)]
    [DisplayName("Role Name")]
    public string? RoleName { get; set; }

    [Required]
    [StringLength(30)]
    [DisplayName("Code")]
    public string? Code { get; set; }
}