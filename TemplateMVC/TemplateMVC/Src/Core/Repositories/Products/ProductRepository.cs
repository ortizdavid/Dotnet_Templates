using TemplateMVC.Core.Models;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models.Products;
using System.Data;
using Dapper;

namespace TemplateMVC.Core.Repositories;

public class ProductRepository : RepositoryBase<Product>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;
    private static readonly Dictionary<string, string> _sortOptions = new()
    {
        ["name_asc"] = "ProductName ASC",
        ["name_desc"] = "ProductName DESC",
        ["code_asc"] = "Code ASC",
        ["code_desc"] = "Code DESC",
        ["category_asc"] = "CategoryName ASC",
        ["category_desc"] = "CategoryName DESC",
        ["supplier_asc"] = "SupplierName ASC",
        ["supplier_desc"] = "SupplierName DESC",
    };

    public ProductRepository(AppDbContext context, IDbConnection ddapper) : base(context)
    {
        _context = context;
        _dapper = ddapper;
    }

    public async Task<IEnumerable<ProductData>> GetAllDataAsync(int pageSize, int pageIndex)
    {
        int offset = pageIndex * pageSize; 
        var sql = @"SELECT * FROM ViewProductData 
                ORDER BY UserId ASC 
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var parameters = new { Offset = offset, PageSize = pageSize };
        return await _dapper.QueryAsync<ProductData>(sql, parameters);
    }

    public async Task<IEnumerable<ProductData>> GetAllDataSortedAsync(int pageSize, int pageIndex, string searchString, string sortOrder)
    {
        int offset = pageIndex * pageSize;
        var parameters = new DynamicParameters();
        var filter = "";

        if (!string.IsNullOrEmpty(searchString))
        {
            filter = @"WHERE ProductName LIKE @SearchParam 
                        OR Code LIKE @SearchParam 
                        OR CategoryName LIKE @SearchParam 
                        OR SupplierName LIKE @SearchParam";
            parameters.Add("SearchParam", $"%{searchString ?? ""}%");
        } 

        var sort = _sortOptions.GetValueOrDefault(sortOrder, "ProductId ASC");
        var sql = @$"SELECT * FROM ViewProductData 
                    {filter} ORDER BY {sort} 
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        parameters.Add("Offset", offset);
        parameters.Add("PageSize", pageSize);
        return await _dapper.QueryAsync<ProductData>(sql, parameters);
    }

    public async Task<Product?> GetByCodeAsync(string? code)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Code == code);    
    }

    public async Task<ProductData> GetDataByIdAsync(int id)
    {
        var sql = "SELECT * FROM ViewProductData WHERE ProductId = @Id";
        return await _dapper.QueryFirstAsync<ProductData>(sql, new { Id = id });
    }

    public async Task<ProductData> GetDataByUniqueIdAsync(Guid uniqueId)
    {
        var sql = "SELECT * FROM ViewProductData WHERE UniqueId = @Id";
        return await _dapper.QueryFirstAsync<ProductData>(sql, new { Id = uniqueId });
    }

    public async Task<IEnumerable<Product>> GetAllBySupplierAsync(int id)
    {
        return await _context.Products
            .OrderBy(p => p.ProductId)
            .Where(s => s.SupplierId == id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> ExistsRecordExcluded(string? code, Guid excludedId)
    {
        return await _dbSet.AnyAsync(p => 
            p.Code == code && p.UniqueId != excludedId
        );
    }
}
