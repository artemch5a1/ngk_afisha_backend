using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class EventTypeEntity : IEntity<EventTypeEntity, EventType>
{
    [Column("type_id")]
    public int TypeId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    private EventTypeEntity(EventType eventType)
    {
        Title = eventType.Title;
    }

    internal EventTypeEntity()
    {
        
    }

    public EventType ToDomain()
    {
        return EventType.Restore(TypeId, Title);
    }

    public static EventTypeEntity ToEntity(EventType domain)
    {
        return new EventTypeEntity(domain);
    }
}