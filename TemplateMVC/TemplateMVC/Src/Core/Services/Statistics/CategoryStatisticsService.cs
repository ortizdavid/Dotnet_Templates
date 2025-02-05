using TemplateMVC.Core.Models.Statistics;
using TemplateMVC.Core.Repositories.Statistics;

namespace TemplateMVC.Core.Services.Statistics;

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