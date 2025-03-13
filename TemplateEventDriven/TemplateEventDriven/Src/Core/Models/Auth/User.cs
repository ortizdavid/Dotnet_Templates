using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;
using TemplateEventDriven.Common.Helpers;

namespace TemplateEventDriven.Core.Models.Auth;

[EFIndex(nameof(UserName), IsUnique = true)]
[EFIndex(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [ForeignKey("FK_Role")]
    public int RoleId { get; set; }

    [Required]
    [StringLength(100)]
    public string? UserName { get; set; }

    [Required]
    [StringLength(150)]
    public string? Email { get; set; }

    [Required]
    [StringLength(150)]
    public string? Password { get; set; }

    public bool IsActive { get; set; } = true;

    [StringLength(100)]
    public string? Image { get; set; }

    [StringLength(200)]
    public string? RecoveryToken { get; set; } = Encryption.GenerateRandomToken(150);

    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
