using System.ComponentModel.DataAnnotations;

namespace TemplateEventDriven.Core.Models.Events;

public class ProductEvent
{
    [Key]
    public int EventId { get; set; }
    public int ProductId { get; set; }
    public EventTypeEnum EventType { get; set; }
    public string? EventData { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}