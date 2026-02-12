namespace EventService.API.Contracts.EventTypes;

public class UpdateEventTypeDto
{
    public int TypeId { get; set; }

    public string Title { get; set; } = null!;
}
