using System.Text.Json;

namespace AppConsumer.Models;

public class Category 
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public Guid UniqueId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}