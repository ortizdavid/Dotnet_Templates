namespace TemplateMongoDbApi.Core.Models.Auth;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            UserId = user.UserId.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            IsActive = user.IsActive,
            RecoveryToken = user.RecoveryToken,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Role = RoleMapper.ToResponse(user.Role),
            UserRefreshToken = user.UserRefreshToken,
        };
    }

    public static List<UserResponse> ToResponseList(IEnumerable<User> users)
    {
        return users.Select(u => new UserResponse
        {
            UserId = u.UserId.ToString(),
            UserName = u.UserName,
            Email = u.Email,
            IsActive = u.IsActive,
            RecoveryToken = u.RecoveryToken,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt,
            Role = RoleMapper.ToResponse(u.Role),
            UserRefreshToken = u.UserRefreshToken,
        }).ToList();
    }
}
