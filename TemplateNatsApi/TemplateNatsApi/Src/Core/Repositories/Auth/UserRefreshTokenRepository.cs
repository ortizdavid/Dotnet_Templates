using Microsoft.EntityFrameworkCore;
using TemplateNatsApi.Core.Models;
using TemplateNatsApi.Core.Models.Auth;

namespace TemplateNatsApi.Core.Repositories.Auth;

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