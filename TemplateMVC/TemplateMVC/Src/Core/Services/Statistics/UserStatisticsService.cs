using TemplateMVC.Core.Repositories.Statistics;
using TemplateMVC.Core.Models.Statistics;

namespace TemplateMVC.Core.Services.Statistics;

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