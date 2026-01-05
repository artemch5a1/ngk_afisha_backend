using EventService.API.Contracts.EventRole;
using EventService.API.Contracts.Events;
using EventService.Domain.Enums;

namespace EventService.API.Contracts.Invitation;

public class InvitationDto
{
    public Guid InvitationId { get; set; }

    public Guid EventId { get; set; }

    public EventDto Event { get; set; } = null!;

    public int RoleId { get; set; }

    public EventRoleDto Role { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int RequiredMember { get; set; }

    public int AcceptedMember { get; set; }
    
    public DateTime DeadLine { get; set; }

    public InvitationStatus Status { get; set; }
}