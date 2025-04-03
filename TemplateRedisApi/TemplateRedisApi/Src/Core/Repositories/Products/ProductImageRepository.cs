using TemplateRedisApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using TemplateRedisApi.Core.Models.Products;

namespace TemplateRedisApi.Core.Repositories.Products;

public class ProductImageRepository : RepositoryBase<ProductImage>
{
    protected readonly AppDbContext _context;

    public ProductImageRepository(AppDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task DeleteByProductAsync(int productId)
    {
        try
        {
            var images = await GetAllByProductAsync(productId);
            _dbSet.RemoveRange(images);
            await _context.SaveChangesAsync(); 
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<ProductImage>> GetAllByProductAsync(int productId)
    {
        var images = await _dbSet
                .OrderBy(img => img.ImageId)
                .Where(img => img.ProductId == productId)
                .ToListAsync();
        return images;
    }
}
