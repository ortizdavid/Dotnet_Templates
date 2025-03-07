using TemplateEventDriven.Core.Models.Statistics;
using TemplateEventDriven.Core.Repositories.Statistics;

namespace TemplateEventDriven.Core.Services.Statistics;

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