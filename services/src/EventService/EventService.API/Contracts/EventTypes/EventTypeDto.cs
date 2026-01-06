namespace EventService.API.Contracts.EventTypes;

public class EventTypeDto
{
    public int TypeId { get; set; }

    public string Title { get; set; } = null!;
}