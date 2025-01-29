using TemplateApi.Core.Models.Suppliers;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;
using System.Data;
using Dapper;

namespace TemplateApi.Core.Repositories.Suppliers;

public class SupplierRepository : IRepository<Supplier>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    public SupplierRepository(AppDbContext context, IDbConnection dapper)
    {
        _context = context;
        _dapper = dapper;
    }

    public async Task CreateAsync(Supplier entity)
    {
        try
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateBatchAsync(IEnumerable<Supplier> entities)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await _context.Suppliers.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            };
        }
    }

    public async Task UpdateAsync(Supplier entity)
    {
        try
        {
            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(Supplier entity)
    {
        try
        {
            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync(int limit, int offset)
    {
        return await _context.Suppliers
            .OrderBy(s => s.SupplierId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Suppliers.FindAsync(id);
    }

    public async Task<Supplier?> GetByUniqueIdAsync(Guid uniqueId)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.UniqueId == uniqueId);
    }

    public async Task<Supplier?> GetByIdentNumberAsync(string? identNumber)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.IdentificationNumber == identNumber);    
    }

    public async Task<bool> ExistsRecord(string? fieldName, string? value)
    {
        var sql = $"SELECT 1 FROM Suppliers WHERE {fieldName} = @Value";
        var count = await _dapper.ExecuteScalarAsync<int>(sql, new { Value = value });
        return count > 0;
    }

    public async Task<int> CountAsync()
    {
        return await _context.Suppliers.CountAsync();
    }
}
