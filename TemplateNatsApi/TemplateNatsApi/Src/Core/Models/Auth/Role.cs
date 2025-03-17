using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TemplateNatsApi.Common.Helpers;

namespace TemplateNatsApi.Core.Models.Auth;

public class Role : IModel
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

    // RelationShips
    public ICollection<User> Users { get; set; } = new List<User>(); 

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.RoleName).IsUnique();
            entity.HasIndex(e => e.Code);
        });
    }
}