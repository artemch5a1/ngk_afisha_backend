using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Models;
using EventService.Infrastructure.Entites;

namespace EventService.Infrastructure.Implementations.Mapping;

public class EventMapper : IEntityMapper<EventEntity, Event>
{
    public void BeforeMapping(ref Event domain, EventEntity entity)
    {
        if (entity.Location is not null)
            AddLocationNavigation(ref domain, entity.Location);

        if (entity.Genre is not null)
            AddGenreNavigation(ref domain, entity.Genre);

        if (entity.Type is not null)
            AddTypeNavigation(ref domain, entity.Type);

        AddInvitationNavigation(ref domain, entity.Invitations);
    }

    private void AddInvitationNavigation(
        ref Event domain,
        ICollection<InvitationEntity> invitationsEntities
    )
    {
        List<Invitation> invitations = invitationsEntities.Select(x => x.ToDomain()).ToList();

        foreach (var t in invitations)
        {
            Invitation invitation = t;

            InvitationEntity invitationEntity = invitationsEntities.First(x =>
                x.InvitationId == invitation.InvitationId
            );

            AddMemberNavigation(ref invitation, invitationEntity.Members);
        }

        domain.AddInvitationNavigation(invitations);
    }

    private void AddMemberNavigation(
        ref Invitation domain,
        ICollection<MemberEntity> memberEntities
    )
    {
        List<Member> members = memberEntities.Select(x => x.ToDomain()).ToList();

        domain.AddMemberNavigation(members);
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
