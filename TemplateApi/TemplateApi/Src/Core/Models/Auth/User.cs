using System.ComponentModel.DataAnnotations;
using TemplateApi.Helpers;

namespace TemplateApi.Core.Models.Auth;

public class User
{
    [Key]
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Image { get; set; }
    public string? RecoveryToken { get; set; } = Encryption.GenerateRandomToken(150);
    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
