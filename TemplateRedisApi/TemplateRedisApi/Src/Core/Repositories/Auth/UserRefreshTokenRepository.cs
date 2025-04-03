using Microsoft.EntityFrameworkCore;
using TemplateRedisApi.Core.Models;
using TemplateRedisApi.Core.Models.Auth;

namespace TemplateRedisApi.Core.Repositories.Auth;

public class UserRefreshTokenRepository : RepositoryBase<UserRefreshToken>
{
    private readonly AppDbContext _context;
    public UserRefreshTokenRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<UserRefreshToken?> GetByUserIdAsync(int userId)
    {
        return await _context.UserRefreshTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }
}