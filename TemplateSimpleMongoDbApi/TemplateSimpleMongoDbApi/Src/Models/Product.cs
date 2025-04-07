using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateSimpleMongoDbApi.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [BsonElement("price")]
    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }
}