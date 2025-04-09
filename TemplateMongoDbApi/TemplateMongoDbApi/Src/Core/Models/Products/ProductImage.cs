using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateMongoDbApi.Core.Models.Products;

public class ProductImage 
{   
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ImageId { get; set; }

    [BsonElement("product_id")]
    public ObjectId ProductId { get; set; }

    [BsonElement("file_name")]
    public string? FileName { get; set; }

    [BsonElement("upload_dir")]
    public string? UploadDir { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
