using System.ComponentModel.DataAnnotations;

namespace TemplateEventDriven.Core.Models.Events;

public class EventType
{
    [Key]
    public int TypeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public enum EventTypeEnum
{
    Created = 1,
    Updated,
    Deleted,
    Imported,
    Processed,
    FailedProcessing,
    FailedImport,
    Exported,
    Published,
    Consumed
}