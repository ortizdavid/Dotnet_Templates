namespace TemplateEventDriven.Core.Models.Auth;

public  class UserMessage
{
    public Guid UniqueId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Role { get; set; }
}