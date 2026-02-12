using EventService.API.Contracts.EventTypes;
using EventService.API.Contracts.Genres;
using EventService.API.Contracts.Locations;

namespace EventService.API.Contracts.Events;

public class CreatedEventDto
{
    public Guid EventId { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public DateTime DateStart { get; set; }

    public int LocationId { get; set; }

    public LocationDto Location { get; set; } = null!;

    public int GenreId { get; set; }

    public GenreDto Genre { get; set; } = null!;

    public int TypeId { get; set; }

    public EventTypeDto Type { get; set; } = null!;

    public int MinAge { get; set; }

    internal Guid Author { get; set; }

    public string UploadUrl { get; set; } = null!;
}
