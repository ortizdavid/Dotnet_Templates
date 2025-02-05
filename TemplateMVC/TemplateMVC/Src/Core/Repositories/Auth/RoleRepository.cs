using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;
using TemplateMVC.Core.Models.Auth;

namespace TemplateMVC.Core.Repositories.Auth;

public class RoleRepository : RepositoryBase<Role>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    public RoleRepository(AppDbContext context, IDbConnection dapper) : base(context)
    {
        _context = context;
        _dapper = dapper;
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