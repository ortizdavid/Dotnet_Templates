namespace TemplateMVC.Core.Models.Auth;

public class UserData
{
    public int UserId { get; set; }
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public string? Image { get; set; }
    public string? RecoveryToken { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? RoleCode { get; set; }
}