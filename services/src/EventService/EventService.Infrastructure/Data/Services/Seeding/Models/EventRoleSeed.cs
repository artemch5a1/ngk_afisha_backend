namespace EventService.Infrastructure.Data.Services.Seeding.Models;

public class EventRoleSeed
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class EventRoleSeedOptions
{
    public List<EventRoleSeed> EventRoleSeed { get; set; } = [];
}
