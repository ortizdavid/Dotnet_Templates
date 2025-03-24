namespace TemplateEventDriven.Core.Models.Auth;

public class UserCreated
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; } 
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserChangedPassword
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserUploadedImage
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Image { get; set; }
    public string? UploadDir { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserActivated
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserDeactivated
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserDeleted
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public DateTime DeletedAt { get; set; }
}