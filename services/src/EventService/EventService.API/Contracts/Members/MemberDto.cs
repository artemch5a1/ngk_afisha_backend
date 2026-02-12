using EventService.API.Contracts.Invitation;
using EventService.Domain.Enums;

namespace EventService.API.Contracts.Members;

public class MemberDto
{
    public Guid InvitationId { get; set; }

    public InvitationDto Invitation { get; set; } = null!;

    public Guid StudentId { get; set; }

    public MemberStatus Status { get; set; }
}
