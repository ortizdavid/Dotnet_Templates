using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TemplateMongoDbApi.Core.Models.Suppliers;

namespace TemplateMongoDbApi.Core.Models.Products;

public class Product
{   
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ProductId { get; set; }

    [BsonElement("product_name")]
    public string? ProductName { get; set; }

    [BsonElement("code")]
    public string? Code { get; set; }

    [BsonElement("unit_price")]
    public decimal UnitPrice { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("category")]
    public Category? Category { get; set; }

    [BsonElement("supplier")]
    public Supplier? Supplier { get; set; }
}
