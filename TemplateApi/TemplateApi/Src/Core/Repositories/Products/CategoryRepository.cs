using TemplateApi.Core.Models.Products;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;
using System.Data;
using Dapper;

namespace TemplateApi.Core.Repositories.Products;

public class CategoryRepository : IRepository<Category>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    public CategoryRepository(AppDbContext context, IDbConnection ddapper)
    {
        _context = context;
        _dapper = ddapper;
    }

    public async Task CreateAsync(Category entity)
    {
        try
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateBatchAsync(IEnumerable<Category> entities)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await _context.Categories.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task UpdateAsync(Category entity)
    {
        try
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(Category entity)
    {
        try
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Category>> GetAllAsync(int limit, int offset)
    {
        return await _context.Categories
            .Distinct()
            .OrderBy(c => c.CategoryId)
            .Skip(offset) 
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category?> GetByUniqueIdAsync(Guid uniqueId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.UniqueId == uniqueId);
    }

    public async Task<bool> ExistsRecord(string? fieldName, string? value)
    {
        var sql = $"SELECT 1 FROM Categories WHERE {fieldName} = @Value";
        var count = await _dapper.ExecuteScalarAsync<int>(sql, new { Value = value });
        return count > 0;
    }

    public async Task<Category?> GetByNameAsync(string? name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryName == name);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Categories.CountAsync();
    }
}
