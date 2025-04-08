using MongoDB.Bson;
using MongoDB.Driver;

namespace TemplateMongoDbApi.Core.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T : class
{
    private readonly IMongoDatabase _database;
    protected readonly IMongoCollection<T> _collection;
    protected readonly FilterDefinitionBuilder<T> _builder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _database = database;
        _collection = _database.GetCollection<T>(collectionName);
    }

    public async Task<long> CountAsync()
    {
        return await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty);
    }

    public async Task CreateAsync(T entity)
    {
        try
        {
            await _collection.InsertOneAsync(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateBatchAsync(IEnumerable<T> entities)
    {
        try
        {
            await _collection.InsertManyAsync(entities);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            var filter = _builder.Eq("_id", GetEntityId(entity));
            await _collection.DeleteOneAsync(filter);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ExistsRecord(string field, string? value)
    {
        var filter = _builder.Eq(field, value);
        return await _collection.CountDocumentsAsync(filter) > 0;
    }

    public async Task<IEnumerable<T>> GetAllAsync(int pageSize, int pageIndex)
    {
        if (pageSize <= 0 || pageIndex < 0)
        {
            throw new ArgumentException("Invalid pagination parameters.");
        }
        var filter = _builder.Empty;
        return await _collection.Find(filter)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllNotPaginatedAsync()
    {
        var filter = _builder.Empty;
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            var filter = _builder.Eq("_id", GetEntityId(entity));
            await _collection.ReplaceOneAsync(filter, entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<T?> GetByFieldAsync(string field, object value)
    {
        var filter = _builder.Eq(field, value);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(string? id)
    {
        if(!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.Eq("_id", objectId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    private object GetEntityId(T entity)
    {
        var property = typeof(T).GetProperty("_id");
        return property?.GetValue(entity) ?? throw new InvalidOperationException("Entity does not have an _id property.");
    }
}