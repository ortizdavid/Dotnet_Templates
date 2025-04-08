using TemplateMongoDbApi.Core.Models.Products;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TemplateMongoDbApi.Core.Repositories;

public class ProductRepository : MongoRepository<Product>
{
    public ProductRepository(IMongoDatabase database) : base(database, "products")
    {
    }

    public async Task<IEnumerable<Product>> GetAllDataAsync(int pageSize, int pageIndex)
    {
        if (pageSize <= 0 || pageIndex < 0)
        {
            throw new ArgumentException("Invalid pagination parameters.");
        }
        int skip = pageIndex * pageSize; 
        var result = await _collection
            .Find(new BsonDocument())
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();
        return result;
    }

    public async Task<Product> GetByCodeAsync(string? code)
    {
        var filter = _builder.Eq(p => p.Code, code);
        return await _collection.Find(filter).FirstOrDefaultAsync();   
    }

    public async Task<Product> GetDataByIdAsync(string id)
    {
        if(!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.Eq(p => p.ProductId, objectId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetAllBySupplierAsync(string id)
    {
        if(!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.Eq(p => p.Supplier!.SupplierId, objectId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<bool> ExistsRecordExcluded(string? code, string excludedId)
    {
        if(!ObjectId.TryParse(excludedId, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        
        var filter = _builder.Eq(p => p.ProductId, objectId) & 
                     _builder.Ne(p => p.ProductId, objectId);
        return await _collection.Find(filter).AnyAsync();
    }
}
