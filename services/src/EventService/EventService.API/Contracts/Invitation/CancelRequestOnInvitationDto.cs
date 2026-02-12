namespace EventService.API.Contracts.Invitation;

public class CancelRequestOnInvitationDto
{
    public Guid EventId { get; set; }

    public Guid InvitationId { get; set; }
}
