using TemplateMVC.Core.Models.Auth;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;
using System.Data;
using Dapper;

namespace TemplateMVC.Core.Repositories.Auth;

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
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
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
