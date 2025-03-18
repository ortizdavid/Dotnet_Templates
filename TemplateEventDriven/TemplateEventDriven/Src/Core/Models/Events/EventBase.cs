using System.ComponentModel.DataAnnotations;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace TemplateEventDriven.Core.Models.Events;

[EFIndex(nameof(EntityId))]
public class EventBase
{
    [Key]
    public int EventId { get; set; }
    
    public int? EntityId { get; set; }

    [Required]
    public EventTypeEnum EventType { get; set; }

    [StringLength(100)]
    public string? ActionName { get; set; }

    public string? EventData { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}