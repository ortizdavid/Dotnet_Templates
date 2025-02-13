using System.Data;
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