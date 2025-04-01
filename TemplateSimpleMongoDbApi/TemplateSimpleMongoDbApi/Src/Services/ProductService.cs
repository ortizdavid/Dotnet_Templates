using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TemplateSimpleMongoDbApi.Models;

namespace TemplateSimpleMongoDbApi.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _collection;

    public ProductService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Product>("products");
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var products = await _collection.Find(_ => true).ToListAsync();
        return products;
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        var product = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product;
    }

    public async Task CreateAsync(Product newProduct)
    {
        await _collection.InsertOneAsync(newProduct);
    }

    public async Task UpdateAsync(string id, Product updatedProduct)
    {
        await _collection.ReplaceOneAsync(p => p.Id == id, updatedProduct);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(p => p.Id == id);
    }

    public async Task<bool> Exists(string field, string value) 
    {
        var filter = Builders<Product>.Filter.Eq(field, value);
        return await _collection.Find(filter).AnyAsync();
    }
}