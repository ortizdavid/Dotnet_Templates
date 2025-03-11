using TemplateEventDriven.Common.Messaging.RabbitMQ;
using TemplateEventDriven.Core.Models.Events;
using TemplateEventDriven.Core.Repositories;
using System.Text.Json;

namespace TemplateEventDriven.Core.Services.Events;

public class EventService<T> where T: EventBase, new()
{
    private readonly RabbitMQProducer _rabbitMQProducer;
    private readonly RepositoryBase<T> _repository;

    public EventService(RabbitMQProducer rabbitMQProducer, RepositoryBase<T> repository)
    {
        _rabbitMQProducer = rabbitMQProducer;
        _repository = repository;
    }

    public async Task PublishCreatedEvent(int? entityId, string exchange, object newObj, string routingKey)
    {
        var eventObj = new T
        {
            EntityId = entityId,
            EventType = EventTypeEnum.Created,
            EventData = SerializeData(newObj),
            CreatedAt = DateTime.UtcNow
        };
        await _repository.CreateAsync(eventObj);
        await _rabbitMQProducer.PublishToExchange(exchange, newObj, routingKey);
    }

    public async Task PublishUpdatedEvent(int? entityId, string exchange,  object before, object after, string routingKey)
    {
        var eventObj = new T
        {
            EntityId = entityId,
            EventType = EventTypeEnum.Updated,
            EventData = SerializeData(new { Before = before, After = after }),
            CreatedAt = DateTime.UtcNow
        };
        await _repository.CreateAsync(eventObj);
        await _rabbitMQProducer.PublishToExchange(exchange, after, routingKey);
    }

    public async Task PublishDeletedEvent(int? entityId, string exchange,  object deletedObj, string routingKey)
    {
        var eventObj = new T
        {
            EntityId = entityId,
            EventType = EventTypeEnum.Deleted,
            EventData = SerializeData(deletedObj),
            CreatedAt = DateTime.UtcNow
        };
        await _repository.CreateAsync(eventObj);
        await _rabbitMQProducer.PublishToExchange(exchange, deletedObj, routingKey);
    }

    private string SerializeData(object eventData)
    {
        return JsonSerializer.Serialize(eventData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
            WriteIndented = false
        });
    }
}