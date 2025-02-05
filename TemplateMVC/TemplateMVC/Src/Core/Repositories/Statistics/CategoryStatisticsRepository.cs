using System.Data;
using Dapper;
using TemplateMVC.Core.Models.Statistics;

namespace TemplateMVC.Core.Repositories.Statistics;

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