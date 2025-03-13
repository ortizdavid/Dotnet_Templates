using System.ComponentModel.DataAnnotations;
using TemplateEventDriven.Common.Helpers;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace TemplateEventDriven.Core.Models.Auth;

[EFIndex(nameof(RoleName), IsUnique = true)]
[EFIndex(nameof(Code), IsUnique = true)]
public class Role
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    [StringLength(100)]
    public string? RoleName { get; set; }

    [Required]
    [StringLength(30)]
    public string? Code { get; set; }

    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}