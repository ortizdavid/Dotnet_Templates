using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;
using TemplateApi.Core.Models.Auth;

namespace TemplateApi.Core.Repositories.Auth
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _dapper;

        public RoleRepository(AppDbContext context, IDbConnection dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Roles.CountAsync();
        }

        public async Task CreateAsync(Role entity)
        {
            try
            {
                await _context.Roles.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateBatchAsync(IEnumerable<Role> entities)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Roles.AddRangeAsync(entities);
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

        public async Task DeleteAsync(Role entity)
        {
            try
            {
                _context.Roles.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ExistsRecord(string? fieldName, string? value)
        {
            var sql = $"SELECT 1 FROM Roles WHERE {fieldName} = @Value";
            var count = await _dapper.ExecuteScalarAsync<int>(sql, new { Value = value });
            return count > 0;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(int limit, int offset)
        {
            return await _context.Roles
                .OrderBy(r => r.RoleName)
                .Skip(limit)
                .Take(offset)
                .ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<Role?> GetByUniqueIdAsync(Guid uniqueId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.UniqueId == uniqueId);
        }

        public async Task UpdateAsync(Role entity)
        {
            try
            {
                _context.Roles.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllDataAsync(int limit, int offset)
        {
            var sql = "SELECT * FROM Roles ORDER BY CreatedAt DESC " + 
                $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;";
            return await _dapper.QueryAsync<Role>(sql);
        }

        public async Task<IEnumerable<Role>> GetAllNoLimitAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetByCodeAsync(string code)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Code == code);
        }
    }
}