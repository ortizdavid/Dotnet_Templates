using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class CreateRoleViewModel
{
    [Required]
    [StringLength(150)]
    [DisplayName("Role Name")]
    public string? RoleName { get; set; }

    [Required]
    [StringLength(30)]
    [DisplayName("Code")]
    public string? Code { get; set; }
}