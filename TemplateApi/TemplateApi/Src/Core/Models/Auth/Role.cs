using System.ComponentModel.DataAnnotations;
using TemplateApi.Helpers;

namespace TemplateApi.Core.Models.Auth
{
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
}