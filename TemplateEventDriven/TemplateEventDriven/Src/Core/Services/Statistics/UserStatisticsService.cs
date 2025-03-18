using TemplateEventDriven.Core.Repositories.Statistics;
using TemplateEventDriven.Core.Models.Statistics;

namespace TemplateEventDriven.Core.Services.Statistics;

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