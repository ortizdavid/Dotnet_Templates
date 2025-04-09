namespace TemplateMongoDbApi.Core.Models.Auth;

public class UserResponse
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Image { get; set; }
    public string? RecoveryToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public RoleResponse? Role { get; set; }
    public UserRefreshToken? UserRefreshToken { get; set; } 
}
