using System.Data;
using Dapper;
using TemplateNatsApi.Core.Models.Statistics;

namespace TemplateNatsApi.Core.Repositories.Statistics;

public class CategoryStatisticsRepository
{
    private readonly IDbConnection _dapper;

    public CategoryStatisticsRepository(IDbConnection dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<CategoryTopCategories>> GetCategoryTopCategoriesAsync()
    {
        var sql = "SELECT * FROM ViewCategoryTopCategories;";
        return await _dapper.QueryAsync<CategoryTopCategories>(sql);
    }  
}