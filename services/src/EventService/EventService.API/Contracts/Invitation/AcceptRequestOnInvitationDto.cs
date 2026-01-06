namespace EventService.API.Contracts.Invitation;

public class AcceptRequestOnInvitationDto
{
    public Guid EventId { get; set; }

    public Guid InvitationId { get; set; }
    
    public Guid StudentId { get; set; }
}