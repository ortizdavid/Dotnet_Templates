namespace TemplateMongoDbApi.Core.DTOs.Auth;

public class RoleResponse
{
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Code { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } 
}