using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Models;
using EventService.Infrastructure.Entites;

namespace EventService.Infrastructure.Implementations.Mapping;

public class InvitationMapper : IEntityMapper<InvitationEntity, Invitation>
{
    public void BeforeMapping(ref Invitation domain, InvitationEntity entity)
    {
        if (entity.Event is not null)
            AddEventNavigation(ref domain, entity.Event);

        if (entity.Role is not null)
            AddEventRoleNavigation(ref domain, entity.Role);
    }

    private void AddEventNavigation(ref Invitation domain, EventEntity eventEntity)
    {
        Event @event = eventEntity.ToDomain();

        if (eventEntity.Location is not null)
            AddLocationNavigation(ref @event, eventEntity.Location);

        if (eventEntity.Genre is not null)
            AddGenreNavigation(ref @event, eventEntity.Genre);

        if (eventEntity.Type is not null)
            AddTypeNavigation(ref @event, eventEntity.Type);

        domain.AddEventNavigation(@event);
    }

    private void AddEventRoleNavigation(ref Invitation domain, EventRoleEntity roleEntity)
    {
        EventRole eventRole = roleEntity.ToDomain();

        domain.AddRoleNavigation(eventRole);
    }

    private void AddLocationNavigation(ref Event domain, LocationEntity locationEntity)
    {
        Location location = locationEntity.ToDomain();

        domain.AddLocationNavigation(location);
    }

    private void AddTypeNavigation(ref Event domain, EventTypeEntity eventTypeEntity)
    {
        EventType eventType = eventTypeEntity.ToDomain();

        domain.AddEventTypeNavigation(eventType);
    }

    private void AddGenreNavigation(ref Event domain, GenreEntity genreEntity)
    {
        Genre genre = genreEntity.ToDomain();

        domain.AddGenreNavigation(genre);
    }
}
