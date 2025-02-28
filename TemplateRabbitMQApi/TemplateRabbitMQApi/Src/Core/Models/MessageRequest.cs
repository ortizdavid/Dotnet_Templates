using System.Text.Json;

namespace TemplateRabbitMQApi.Core.Models;

public class MessageRequest
{
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Identification { get; set; } = string.Empty;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}