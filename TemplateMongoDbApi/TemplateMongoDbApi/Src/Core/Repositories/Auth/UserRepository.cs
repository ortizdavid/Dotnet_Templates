using MongoDB.Bson;
using MongoDB.Driver;
using TemplateMongoDbApi.Core.Models.Auth;

namespace TemplateMongoDbApi.Core.Repositories.Auth;

public class UserRepository : MongoRepository<User>
{
    public UserRepository(IMongoDatabase database) : base(database, "users")
    {
    }

    public async Task<User?> GetByNameAsync(string? userName)
    {
        var filter = _builder.Eq(u => u.UserName, userName);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string? email)
    {
        var filter = _builder.Eq(u => u.Email, email);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetAllDataAsync(int pageSize, int pageIndex)
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

    public async Task<bool> ExistsRecordExcluded(string? userName, string? email, string excludedId)
    {
        if (!ObjectId.TryParse(excludedId, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.And(
            _builder.Or(
                _builder.Eq(u => u.UserName, userName),
                _builder.Eq(u => u.Email, email)
            ),
            _builder.Ne(u => u.UserId, objectId)
        );
        return await _collection.Find(filter).AnyAsync();
    }

    public async Task<User?> GetDataByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.Eq(u => u.UserId, objectId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetDataByNameAsync(string userName)
    {
        var filter = _builder.Eq(u => u.UserName, userName);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetDataByRefreshTokenAsync(string token)
    {
        var filter = _builder.Eq(u => u.UserRefreshToken!.Token, token);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByRecoveryTokenAsync(string token)
    {
        var filter = _builder.Eq(u => u.RecoveryToken, token);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
