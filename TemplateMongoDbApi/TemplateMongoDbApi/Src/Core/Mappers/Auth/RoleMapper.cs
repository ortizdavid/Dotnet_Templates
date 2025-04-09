using TemplateMongoDbApi.Core.DTOs.Auth;
using TemplateMongoDbApi.Core.Models.Auth;

namespace TemplateMongoDbApi.Core.Mappers.Auth;

public static class RoleMapper
{
    public static RoleResponse ToResponse(Role? role)
    {
        return new RoleResponse
        {
            RoleId = role?.RoleId.ToString(),
            RoleName = role?.RoleName,
            Code = role?.Code,
            CreatedAt = role?.CreatedAt,
            UpdatedAt = role?.UpdatedAt
        };
    }

    public static List<RoleResponse> ToResponseList(IEnumerable<Role> roles)
    {
        return roles.Select(r => new RoleResponse
        {
            RoleId = r.RoleId.ToString(),
            RoleName = r.RoleName,
            Code = r.Code,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        }).ToList();
    }
}