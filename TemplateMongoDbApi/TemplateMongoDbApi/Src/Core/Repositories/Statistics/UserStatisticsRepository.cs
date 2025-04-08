using TemplateMongoDbApi.Core.Models.Statistics;

namespace TemplateMongoDbApi.Core.Repositories.Statistics;

public class UserStatisticsRepository
{
    public UserStatisticsRepository()
    {
    }

    public async Task<UserActivesAndInactives> GetUserActiveInactivesAsync()
    {
        var sql = "SELECT * FROM ViewUserActiveInactives;";
        return null;
    }

    public async Task<UserPercentageActivesAndInactives> GetUserPercentageActiveInactivesAsync()
    {
        var sql = "SELECT * FROM ViewUserPercentageActiveInactives;";
        return null;
    }
}
