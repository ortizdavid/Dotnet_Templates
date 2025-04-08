using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TemplateMongoDbApi.Common.Helpers;

namespace TemplateMongoDbApi.Core.Models.Auth;

public class User 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId UserId { get; set; }

    [BsonElement("user_name")]
    public string? UserName { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("password")]
    public string? Password { get; set; }

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;
  
    [BsonElement("image")]
    public string? Image { get; set; }

    [BsonElement("recovery_token")]
    public string? RecoveryToken { get; set; } = Encryption.GenerateRandomToken(150);

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("role")]
    public Role? Role { get; set; }

    [BsonElement("user_refresh_token")]
    public UserRefreshToken? UserRefreshToken { get; set; } 
}
