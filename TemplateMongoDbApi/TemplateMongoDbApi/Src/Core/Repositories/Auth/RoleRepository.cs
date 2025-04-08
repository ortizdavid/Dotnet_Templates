using MongoDB.Bson;
using MongoDB.Driver;
using TemplateMongoDbApi.Core.Models.Auth;

namespace TemplateMongoDbApi.Core.Repositories.Auth;

public class RoleRepository : MongoRepository<Role>
{
    public RoleRepository(IMongoDatabase database) : base(database, "roles")
    {
    }

    public async Task<Role?> GetByCodeAsync(string? code)
    {
        var filter = _builder.Eq(r => r.Code, code);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<bool> ExistsRecordExcluded(string? roleName, string? code, string excludedId)
    {
        if(!ObjectId.TryParse(excludedId, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId.");
        }
        var filter = _builder.And(
            _builder.Or(
                _builder.Eq(r => r.RoleName, roleName),
                _builder.Eq(r => r.Code, code)
            ),
            _builder.Ne(r => r.RoleId, objectId)
        );
        
        return await _collection.Find(filter).AnyAsync();
    }
}