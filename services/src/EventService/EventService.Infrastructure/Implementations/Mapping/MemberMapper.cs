using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Models;
using EventService.Infrastructure.Entites;

namespace EventService.Infrastructure.Implementations.Mapping;

public class MemberMapper : IEntityMapper<MemberEntity, Member>
{
    public void BeforeMapping(ref Member domain, MemberEntity entity)
    {
        if (entity.Invitation is not null)
            AddInvitationNavigation(ref domain, entity.Invitation);
    }

    private void AddInvitationNavigation(ref Member domain, InvitationEntity invitationEntity)
    {
        Invitation invitation = invitationEntity.ToDomain();

        if (invitationEntity.Event is not null)
            AddEventNavigation(ref invitation, invitationEntity.Event);

        if (invitationEntity.Role is not null)
            AddEventRoleNavigation(ref invitation, invitationEntity.Role);

        domain.AddInvitationNavigation(invitation);
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
