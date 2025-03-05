using TemplateApi.Core.Models.Auth;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;
using System.Data;
using Dapper;

namespace TemplateApi.Core.Repositories.Auth;

public class UserRepository : RepositoryBase<User>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    public UserRepository(AppDbContext context, IDbConnection dapper) : base(context)
    {
        _context = context;
        _dapper = dapper;
    }

    public async Task<User?> GetByNameAsync(string? userName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User?> GetByEmailAsync(string? email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<UserData>> GetAllDataAsync(int pageSize, int pageIndex)
    {
        int offset = pageIndex * pageSize; 

        var sql = @"SELECT * FROM ViewUserData 
                    ORDER BY UserId ASC 
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var parameters = new { Offset = offset, PageSize = pageSize };
        return await _dapper.QueryAsync<UserData>(sql, parameters);
    }

    public async Task<bool> ExistsRecordExcluded(string? userName, string? email, Guid excludedId)
    {
        return await _dbSet.AnyAsync(u => 
            (u.UserName == userName || u.Email == email) && u.UniqueId != excludedId
        );
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
        return await _dbSet
            .FirstOrDefaultAsync(u => u.RecoveryToken == token);
    }
}
