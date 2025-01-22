using System.ComponentModel.DataAnnotations;

namespace TemplateApi.Core.Models.Auth
{
    public class UserRefreshToken
    {
        [Key]
        public int RefreshId { get; set; }
        [Required]
        public int UserId { get; set; } 
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
}