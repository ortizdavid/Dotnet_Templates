using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TemplateMongoDbApi.Core.Models.Auth;

public class Role
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId RoleId { get; set; }

    [BsonElement("role_name")]
    public string? RoleName { get; set; }

    [BsonElement("code")]
    public string? Code { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}