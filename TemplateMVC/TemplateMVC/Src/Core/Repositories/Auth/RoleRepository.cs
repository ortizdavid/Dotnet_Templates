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
    private static readonly Dictionary<string, string> _sortOptions = new()
    {
        ["name_asc"] = "RoleName ASC",
        ["name_desc"] = "RoleName DESC",
        ["code_asc"] = "Code ASC", 
        ["code_desc"] = "Code DESC"
    };

    public RoleRepository(AppDbContext context, IDbConnection dapper) : base(context)
    {
        _context = context;
        _dapper = dapper;
    }

    public async Task<IEnumerable<Role>> GetAllSortedAsync(int pageSize, int pageIndex, string searchString, string sortOrder)
    {
        int offset = pageIndex * pageSize;
        var parameters = new DynamicParameters();
        var filter = "";

        if (!string.IsNullOrEmpty(searchString))
        {
            filter = "WHERE RoleName LIKE @SearchParam AND Code LIKE @SearchParam";
            parameters.Add("SearchParam", $"%{searchString ?? ""}%");
        }

        var sort = _sortOptions.GetValueOrDefault(sortOrder, "RoleId ASC");
        var sql = @$"SELECT * FROM Roles
                    {filter} ORDER BY {sort}
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        parameters.Add("Offset", offset);
        parameters.Add("PageSize", pageSize);
        return await _dapper.QueryAsync<Role>(sql, parameters);
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