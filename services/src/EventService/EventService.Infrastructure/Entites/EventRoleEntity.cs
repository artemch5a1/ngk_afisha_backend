using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class EventRoleEntity : IEntity<EventRoleEntity, EventRole>
{
    [Column("event_role_id")]
    public int EventRoleId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    internal EventRoleEntity()
    {
        
    }

    private EventRoleEntity(EventRole eventRole)
    {
        Title = eventRole.Title;
        Description = eventRole.Description;
    }


    public EventRole ToDomain()
    {
        return EventRole.Restore(EventRoleId, Title, Description);
    }

    public static EventRoleEntity ToEntity(EventRole domain)
    {
        return new EventRoleEntity(domain);
    }
}