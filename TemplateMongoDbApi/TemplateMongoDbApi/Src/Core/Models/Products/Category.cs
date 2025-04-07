using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateMongoDbApi.Core.Models.Products;

public class Category 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId CategoryId { get; set; }

    [BsonElement("category_name")]
    public string? CategoryName { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
