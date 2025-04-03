using TemplateMongoDbApi.Core.Models.Products;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TemplateMongoDbApi.Core.Repositories.Products;

public class ProductImageRepository : MongoRepository<ProductImage>
{
    public ProductImageRepository(IMongoDatabase database) : base(database, "productImage")
    {
    }

    public async Task DeleteByProductAsync(ObjectId productId)
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
