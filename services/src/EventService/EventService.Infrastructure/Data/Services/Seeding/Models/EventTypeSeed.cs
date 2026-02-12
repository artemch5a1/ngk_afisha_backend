namespace EventService.Infrastructure.Data.Services.Seeding.Models;

public class EventTypeSeed
{
    public string Title { get; set; } = null!;
}

public class EventTypeSeedOptions
{
    public List<EventTypeSeed> EventTypeSeed { get; set; } = [];
}
