using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TemplateApi.Core.Models.Statistics;

namespace TemplateApi.Core.Repositories.Statistics
{
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
}
