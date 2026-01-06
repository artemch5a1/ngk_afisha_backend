namespace EventService.API.Contracts.Invitation;

public class CreateInvitationDto
{
    public Guid EventId { get; set; }

    public int RoleId { get; set; }

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int RequiredMember { get; set; }
    
    public DateTime DeadLine { get; set; }
}