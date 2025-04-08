using MongoDB.Bson;
using MongoDB.Driver;
using TemplateMongoDbApi.Core.Models.Auth;

namespace TemplateMongoDbApi.Core.Repositories.Auth;

public class UserRefreshTokenRepository : MongoRepository<UserRefreshToken>
{
    public UserRefreshTokenRepository(IMongoDatabase database) : base(database, "user_refresh_token")
    {
    }
    
    public async Task<UserRefreshToken?> GetByUserIdStrAsync(string userId)
    {
        if(!ObjectId.TryParse(userId, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.Eq(rt => rt.UserId, objectId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<UserRefreshToken?> GetByUserIdAsync(ObjectId userId)
    {
        var filter = _builder.Eq(rt => rt.UserId, userId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}