using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;
using TemplateApi.Core.Models.Auth;

namespace TemplateApi.Core.Repositories.Auth
{
    public class UserRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public UserRefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserRefreshToken entity)
        {
           try
           {
                await _context.UserRefreshTokens.AddAsync(entity);
                await _context.SaveChangesAsync();
           }
           catch (Exception)
           {
                throw;
           }
        }

        public async Task UpdateAsync(UserRefreshToken entity)
        {
            try
            {
                _context.UserRefreshTokens.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(UserRefreshToken entity)
        {
            try
            {
                _context.UserRefreshTokens.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserRefreshToken?> GetByIdAsync(int id)
        {
            return await _context.UserRefreshTokens
                .FirstOrDefaultAsync(t => t.RefreshId == id);
        }

        public async Task<UserRefreshToken?> GetByUserIdAsync(int userId)
        {
            return await _context.UserRefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }
    }
}