
using TemplateMongoDbApi.Core.Models.Statistics;

namespace TemplateMongoDbApi.Core.Repositories.Statistics;

public class CategoryStatisticsRepository
{
    public CategoryStatisticsRepository()
    {
    }

    public async Task<IEnumerable<CategoryTopCategories>> GetCategoryTopCategoriesAsync()
    {
        var sql = "SELECT * FROM ViewCategoryTopCategories;";
        return null;
    }  
}