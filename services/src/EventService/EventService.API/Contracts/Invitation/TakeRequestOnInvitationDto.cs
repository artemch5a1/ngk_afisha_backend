namespace EventService.API.Contracts.Invitation;

public class TakeRequestOnInvitationDto
{
    public Guid EventId { get; set; }

    public Guid InvitationId { get; set; }
}
