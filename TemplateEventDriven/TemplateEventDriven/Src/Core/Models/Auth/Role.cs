using System.ComponentModel.DataAnnotations;
using TemplateEventDriven.Common.Helpers;

namespace TemplateEventDriven.Core.Models.Auth;

public class Role
{
    [Key]
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Code { get; set; }
    public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}