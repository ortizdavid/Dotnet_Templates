using TemplateRedisApi.Core.Repositories.Statistics;
using TemplateRedisApi.Core.Models.Statistics;

namespace TemplateRedisApi.Core.Services.Statistics;

public class UserStatisticsService
{
    private readonly UserStatisticsRepository _repository;

    public UserStatisticsService(UserStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserActivesAndInactives> GetUserActivesAndInactives()
    {
        return await _repository.GetUserActiveInactivesAsync();
    }

    public async Task<UserPercentageActivesAndInactives> GetUserPercentageActivesAndInactives()
    {
        return await _repository.GetUserPercentageActiveInactivesAsync();
    }
}