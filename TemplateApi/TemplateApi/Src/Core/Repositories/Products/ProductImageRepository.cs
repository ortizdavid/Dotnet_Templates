using TemplateApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models.Products;

namespace TemplateApi.Core.Repositories.Products
{
    public class ProductImageRepository
    {
        protected readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ProductImage image)
        {
            try
            {
                await _context.ProductImages.AddAsync(image);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {   
                throw;
            }
        }

        public async Task CreateBatchAsync(IEnumerable<ProductImage> entities)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.ProductImages.AddRangeAsync(entities);
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

        public async Task DeleteAsync(int productId)
        {
            try
            {
                var images = await _context.ProductImages
                        .Where(img => img.ProductId == productId)
                        .ToListAsync();
                _context.ProductImages.RemoveRange(images);
                await _context.SaveChangesAsync(); 
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductImage>> GetAllAsync(int productId)
        {
            var images = await _context.ProductImages
                    .OrderBy(img => img.ImageId)
                    .Where(img => img.ProductId == productId)
                    .ToListAsync();
            return images;
        }
    }
}
