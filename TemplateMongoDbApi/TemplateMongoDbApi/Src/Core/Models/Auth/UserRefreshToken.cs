using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateMongoDbApi.Core.Models.Auth;

public class UserRefreshToken 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId RefreshId { get; set; }
    
    [BsonElement("user_id")]
    public ObjectId UserId { get; set; } 

    [BsonElement("token")]
    public string? Token { get; set; }

    [BsonElement("expity_date")]
    public DateTime? ExpiryDate { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsExpired => ExpiryDate <= DateTime.UtcNow;
}