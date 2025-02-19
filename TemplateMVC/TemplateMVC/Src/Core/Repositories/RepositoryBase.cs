
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;
using TemplateMVC.Core.Models.Suppliers;

namespace TemplateMVC.Core.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task CreateAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateBatchAsync(IEnumerable<T> entities)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _context.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ExistsRecord(string field, string? value)
    {
        return await _dbSet.AnyAsync(e => EF.Property<string>(e, field) == value);
    }

    public async Task<IEnumerable<T>> GetAllAsync(int pageSize, int pageIndex)
    {
        if (pageSize <= 0 || pageIndex < 0)
        {
            throw new ArgumentException("Invalid pagination parameters.");
        }
        return await _dbSet
            .OrderByDescending(e => EF.Property<DateTime>(e, "CreatedAt")) 
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetByUniqueIdAsync(Guid uniqueId)
    {
        return await _dbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "UniqueId") == uniqueId);
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateBatchAsync(IEnumerable<T> entities)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                _dbSet.UpdateRange(entities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<T?> GetByFieldAsync(string field, object value)
    {
        return await _dbSet.FirstOrDefaultAsync(e => (object)EF.Property<string>(e, field) == value);
    }
}