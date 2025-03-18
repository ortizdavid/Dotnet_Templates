using Microsoft.EntityFrameworkCore;
using TemplateRabbitMQApi.Core.Models;
using TemplateRabbitMQApi.Core.Models.Auth;

namespace TemplateRabbitMQApi.Core.Repositories.Auth;

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