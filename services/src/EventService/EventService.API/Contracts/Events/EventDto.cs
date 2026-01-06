using EventService.API.Contracts.EventTypes;
using EventService.API.Contracts.Genres;
using EventService.API.Contracts.Locations;
using EventService.Domain.Models;

namespace EventService.API.Contracts.Events;

public class EventDto
{
    public Guid EventId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateStart { get; set; }
    
    public int LocationId { get; set; }

    public LocationDto Location { get; set; } = null!;
    
    public int GenreId { get; set; }

    public GenreDto Genre { get; set; } = null!;

    public int TypeId { get; set; }

    public EventTypeDto Type { get; set; } = null!;

    public int MinAge { get; set; }

    public string DownloadUrl { get; set; } = null!;
}