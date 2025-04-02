using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TemplateMongoDbApi.Core.Models;
using TemplateMongoDbApi.Core.Models.Auth;

namespace TemplateMongoDbApi.Core.Repositories.Auth;

public class RoleRepository : RepositoryBase<Role>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    public RoleRepository(AppDbContext context, IDbConnection dapper) : base(context)
    {
        _context = context;
        _dapper = dapper;
    }

    public async Task<Role?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Code == code);
    }

    
    public async Task<bool> ExistsRecordExcluded(string? roleName, string? code, Guid excludedId)
    {
        return await _dbSet.AnyAsync(r => 
            (r.RoleName == roleName || r.Code == code) && r.UniqueId != excludedId
        );
    }
}