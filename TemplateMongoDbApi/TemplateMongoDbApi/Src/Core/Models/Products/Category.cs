using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateMongoDbApi.Core.Models.Products;

public class Category 
{
    [BsonId]
    public ObjectId CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string? CategoryName { get; set; }

    [StringLength(150)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
