using EventService.Domain.Models;

namespace EventService.Application.Contract;

public class CreatedEvent
{
    public Guid EventId { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public DateTime DateStart { get; set; }

    public int LocationId { get; set; }

    public Location Location { get; set; } = null!;

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

    public int TypeId { get; set; }

    public EventType Type { get; set; } = null!;

    public int MinAge { get; set; }

    internal Guid Author { get; set; }

    public string UploadUrl { get; set; } = null!;

    public CreatedEvent(Event @event, string uploadUrl)
    {
        EventId = @event.EventId;
        Title = @event.Title;
        ShortDescription = @event.ShortDescription;
        Description = @event.Description;
        DateStart = @event.DateStart;
        LocationId = @event.LocationId;
        GenreId = @event.GenreId;
        TypeId = @event.TypeId;
        MinAge = @event.MinAge;
        UploadUrl = uploadUrl;
    }
}
