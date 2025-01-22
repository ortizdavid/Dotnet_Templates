using TemplateApi.Core.Models.Auth;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;
using System.Data;
using Dapper;

namespace TemplateApi.Core.Repositories.Auth
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _dapper;

        public UserRepository(AppDbContext context, IDbConnection dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task CreateAsync(User entity)
        {
           try
           {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
           }
           catch (Exception)
           {
                throw;
           }
        }

        public async Task CreateBatchAsync(IEnumerable<User> entities)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Users.AddRangeAsync(entities);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(User entity)
        {
            try
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ExistsRecord(string? fieldName, string? value)
        {
            var sql = $"SELECT 1 FROM Users WHERE {fieldName} = @Value";
            var count = await _dapper.ExecuteScalarAsync<int>(sql, new { Value = value });
            return count > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync(int limit, int offset)
        {
            return await _context.Users
                .OrderBy(u => u.UserId)
                .Skip(limit)
                .Take(offset)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByNameAsync(string? userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User?> GetByUniqueIdAsync(Guid uniqueId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UniqueId == uniqueId);
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<UserData>> GetAllDataAsync(int limit, int offset)
        {
            var sql = "SELECT * FROM ViewUserData ORDER BY CreatedAt DESC " + 
                $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;";
            return await _dapper.QueryAsync<UserData>(sql);
        }

        public async Task<UserData?> GetDataByIdAsync(int id)
        {
            var sql = "SELECT * FROM ViewUserData WHERE UserId = @Id";
            return await _dapper.QueryFirstOrDefaultAsync<UserData>(sql, new { Id = id });
        }

        public async Task<UserData?> GetDataByUniqueIdAsync(Guid uniqueId)
        {
            var sql = "SELECT * FROM ViewUserData WHERE UniqueId = @Id";
            return await _dapper.QueryFirstOrDefaultAsync<UserData>(sql, new { Id = uniqueId });
        }

        public async Task<UserData?> GetDataByNameAsync(string userName)
        {
            var sql = "SELECT * FROM ViewUserData WHERE UserName = @Name";
            return await _dapper.QueryFirstOrDefaultAsync<UserData>(sql, new { Name = userName });
        }

        public async Task<UserData?> GetDataByRefreshTokenAsync(string token)
        {
            var sql = "SELECT * FROM ViewUserData WHERE RefreshToken = @Token";
            return await _dapper.QueryFirstOrDefaultAsync<UserData>(sql, new { Token = token });
        }

        public async Task<User?> GetByRecoveryTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RecoveryToken == token);
        }
    }
}
