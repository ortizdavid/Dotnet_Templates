using TemplateMongoDbApi.Core.Models.Products;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TemplateMongoDbApi.Core.Repositories.Products;

public class ProductImageRepository : MongoRepository<ProductImage>
{
    public ProductImageRepository(IMongoDatabase database) : base(database, "product_images")
    {
    }

    public async Task DeleteByProductAsync(string id)
    {
        try
        {
            if(!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId.");
            }
            var filter = Builders<ProductImage>.Filter.Eq(img => img.ProductId, objectId);
            await _collection.DeleteManyAsync(filter);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<ProductImage>> GetAllByProductAsync(string id)
    {
        if(!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = Builders<ProductImage>.Filter.Eq(img => img.ProductId, objectId);
        var sort = Builders<ProductImage>.Sort.Ascending(img => img.ImageId);
        var images = await _collection.Find(filter).Sort(sort).ToListAsync();
        return images;
    }
}
