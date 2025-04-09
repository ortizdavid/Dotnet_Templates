using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateMongoDbApi.Core.Models.Suppliers;

public class Supplier 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId SupplierId { get; set; }

    [BsonElement("supplier_name")]
    public string? SupplierName { get; set; }

    [BsonElement("identification_number")]
    public string? IdentificationNumber { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }  

    [BsonElement("primary_phone")]
    public string? PrimaryPhone { get; set; } 

    [BsonElement("secondary_phone")]
    public string? SecondaryPhone { get; set; } 

    [BsonElement("address")]
    public string? Address { get; set; }

    [BsonElement("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set;} = DateTime.UtcNow;
}