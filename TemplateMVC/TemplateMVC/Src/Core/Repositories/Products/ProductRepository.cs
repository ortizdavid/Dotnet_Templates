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

    public ProductRepository(AppDbContext context, IDbConnection ddapper) : base(context)
    {
        _context = context;
        _dapper = ddapper;
    }

    public async Task<IEnumerable<ProductData>> GetAllDataAsync(int limit, int offset)
    {
        var sql = "SELECT * FROM ViewProductData ORDER BY CreatedAt DESC " + 
                $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;";
        return await _dapper.QueryAsync<ProductData>(sql);
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
}
