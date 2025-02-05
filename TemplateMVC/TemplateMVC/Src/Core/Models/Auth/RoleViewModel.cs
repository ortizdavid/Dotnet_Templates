using System.ComponentModel.DataAnnotations;

namespace TemplateMVC.Core.Models.Auth;

public class RoleViewModel
{
    [Required]
    [StringLength(150)]
    public string? RoleName { get; set; }

    [Required]
    [StringLength(30)]
    public string? Code { get; set; }
}