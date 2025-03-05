using TemplateRabbitMQApi.Core.Models.Statistics;
using TemplateRabbitMQApi.Core.Repositories.Statistics;

namespace TemplateRabbitMQApi.Core.Services.Statistics;

public class CategoryStatisticsService
{
    private readonly CategoryStatisticsRepository _repository;

    public CategoryStatisticsService(CategoryStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryTopCategories>> GetCategoryTopCategories()
    {
        return await _repository.GetCategoryTopCategoriesAsync();
    }
}