using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TemplateRedisApi.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace TemplateRedisApi.Core.Models.Auth;

public class User : IModel
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

    // RelationShips
    public Role? Role { get; set; }
    public UserRefreshToken? RefreshToken { get; set; } // One to one

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            // Unique Constraints
            entity.HasIndex(e => e.UserName).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            // Foreign Key
            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
