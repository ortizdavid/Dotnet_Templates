using System.Data;
using Dapper;
using TemplateNatsApi.Core.Models.Statistics;

namespace TemplateNatsApi.Core.Repositories.Statistics;

public class UserStatisticsRepository
{
    private readonly IDbConnection _dapper;

    public UserStatisticsRepository(IDbConnection dapper)
    {
        _dapper = dapper;
    }

    public async Task<UserActivesAndInactives> GetUserActiveInactivesAsync()
    {
        var sql = "SELECT * FROM ViewUserActiveInactives;";
        return await _dapper.QueryFirstAsync<UserActivesAndInactives>(sql);
    }

    public async Task<UserPercentageActivesAndInactives> GetUserPercentageActiveInactivesAsync()
    {
        var sql = "SELECT * FROM ViewUserPercentageActiveInactives;";
        return await _dapper.QueryFirstAsync<UserPercentageActivesAndInactives>(sql);
    }
}
