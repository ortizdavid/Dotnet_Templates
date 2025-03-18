using TemplateMVC.Core.Models.Products;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;
using Dapper;
using System.Data;

namespace TemplateMVC.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    private static readonly Dictionary<string, string> _sortOptions = new()
    {
        ["name_asc"] = "CategoryName ASC",
        ["name_desc"] = "CategoryName DESC"
    };

    public CategoryRepository(AppDbContext context, IDbConnection dapper) : base(context)
    {
        _context = context;
        _dapper = dapper;
    }

    public async Task<Category?> GetByNameAsync(string? name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.CategoryName == name);
    }
    
    public async Task<bool> ExistsRecordExcluded(string? categoryName, Guid excludedId)
    {
        return await _dbSet.AnyAsync(c => 
            c.CategoryName == categoryName && c.UniqueId != excludedId
        );
    }

    public async Task<IEnumerable<Category>> GetAllStortedAsync(int pageSize, int pageIndex, string searchString, string sortOrder)
    {
        int offset = pageIndex * pageSize;
        var parameters = new DynamicParameters();
        var filter = "";

        if (!string.IsNullOrEmpty(searchString))
        {
            filter = "WHERE CategoryName LIKE @SearchParam";
            parameters.Add("SearchParam", $"%{searchString ?? ""}%");
        } 

        var sort = _sortOptions.GetValueOrDefault(sortOrder, "CategoryId ASC");
        var sql = @$"SELECT * FROM Categories 
                    {filter} ORDER BY {sort} 
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        parameters.Add("Offset", offset);
        parameters.Add("PageSize", pageSize);
        return await _dapper.QueryAsync<Category>(sql, parameters);
    }
}
