using System.ComponentModel.DataAnnotations;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace TemplateEventDriven.Core.Models.Auth;

[EFIndex(nameof(Token), IsUnique = true)]
public class UserRefreshToken
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

    public bool IsExpired
    {
        get
        {
            return ExpiryDate <= DateTime.UtcNow;
        }
    }
}