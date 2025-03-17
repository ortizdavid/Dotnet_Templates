using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TemplateRabbitMQApi.Core.Models.Auth;

public class UserRefreshToken : IModel
{
    [Key]
    public int RefreshId { get; set; }
    
    [Required]
    public int UserId { get; set; } 

    [StringLength(200)]
    public string? Token { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsExpired => ExpiryDate <= DateTime.UtcNow;
    
    // RelationShips
    public User? User { get; set; }

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.HasIndex(e => e.Token).IsUnique();

            // Foreign key
            entity.HasOne(urt => urt.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<UserRefreshToken>(urt => urt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}