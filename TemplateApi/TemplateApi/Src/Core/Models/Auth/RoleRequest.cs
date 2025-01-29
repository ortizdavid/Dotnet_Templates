using System.ComponentModel.DataAnnotations;

namespace TemplateApi.Core.Models.Auth;

public class RoleRequest
{
    [Required]
    [StringLength(150)]
    public string? RoleName { get; set; }

    [Required]
    [StringLength(30)]
    public string? Code { get; set; }
}