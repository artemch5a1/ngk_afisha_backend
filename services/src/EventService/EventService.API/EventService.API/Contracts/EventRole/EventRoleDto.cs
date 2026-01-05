namespace EventService.API.Contracts.EventRole;

public class EventRoleDto
{
    public int EventRoleId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}