using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFIndex = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace TemplateEventDriven.Core.Models.Events;

[EFIndex(nameof(Name), IsUnique = true)]
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