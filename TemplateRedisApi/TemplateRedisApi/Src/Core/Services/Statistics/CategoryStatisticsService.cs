using TemplateRedisApi.Core.Models.Statistics;
using TemplateRedisApi.Core.Repositories.Statistics;

namespace TemplateRedisApi.Core.Services.Statistics;

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