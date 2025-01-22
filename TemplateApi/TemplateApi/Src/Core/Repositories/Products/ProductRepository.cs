using TemplateApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models.Products;
using System.Data;
using Dapper;

namespace TemplateApi.Core.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _dapper;

        public ProductRepository(AppDbContext context, IDbConnection ddapper)
        {
            _context = context;
            _dapper = ddapper;
        }

        public async Task CreateAsync(Product entity)
        {
            try
            {
                await _context.Products.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateBatchAsync(IEnumerable<Product> entities)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Products.AddRangeAsync(entities);
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
        
        public async Task UpdateAsync(Product entity)
        {
            try
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Product entity)
        {
            try
            {
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int limit, int offset)
        {
            return await _context.Products
                .OrderBy(p => p.ProductId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductData>> GetAllDataAsync(int limit, int offset)
        {
            var sql = "SELECT * FROM ViewProductData ORDER BY CreatedAt DESC " + 
                    $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;";
            return await _dapper.QueryAsync<ProductData>(sql);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        
        public async Task<Product?> GetByUniqueIdAsync(Guid uniqueId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.UniqueId == uniqueId);
        }
        public async Task<Product?> GetByCodeAsync(string? code)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Code == code);    
        }

        public async Task<ProductData?> GetDataByIdAsync(int id)
        {
            var sql = "SELECT * FROM ViewProductData WHERE ProductId = @Id";
            return await _dapper.QueryFirstAsync<ProductData>(sql, new { Id = id });
        }

        public async Task<ProductData?> GetDataByUniqueIdAsync(Guid uniqueId)
        {
            var sql = "SELECT * FROM ViewProductData WHERE UniqueId = @Id";
            return await _dapper.QueryFirstAsync<ProductData>(sql, new { Id = uniqueId });
        }

        public async Task<bool> ExistsRecord(string? fieldName, string? value)
        {
            var sql = $"SELECT 1 FROM Products WHERE {fieldName} = @Value";
            var count = await _dapper.ExecuteScalarAsync<int>(sql, new { Value = value });
            return count > 0;
        }

        public async Task<IEnumerable<Product>> GetAllBySupplierAsync(int id)
        {
            return await _context.Products
                .OrderBy(p => p.ProductId)
                .Where(s => s.SupplierId == id)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }
    }
}
