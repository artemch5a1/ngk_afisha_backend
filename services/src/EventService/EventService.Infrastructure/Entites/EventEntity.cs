using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class EventEntity : IEntity<EventEntity, Event>
{
    [Column("event_id")]
    public Guid EventId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("short_description")]
    public string ShortDescription { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("date_start")]
    public DateTime DateStart { get; set; }

    [Column("location_id")]
    public int LocationId { get; set; }

    [ForeignKey(nameof(LocationId))]
    public LocationEntity Location { get; set; } = null!;

    [Column("genre_id")]
    public int GenreId { get; set; }

    [ForeignKey(nameof(GenreId))]
    public GenreEntity Genre { get; set; } = null!;

    [Column("type_id")]
    public int TypeId { get; set; }

    [ForeignKey(nameof(TypeId))]
    public EventTypeEntity Type { get; set; } = null!;

    [Column("min_age")]
    public int MinAge { get; set; }

    [Column("author")]
    public Guid Author { get; set; }

    [Column("preview_url")]
    public string PreviewUrl { get; set; } = null!;

    public ICollection<InvitationEntity> Invitations { get; set; } = new List<InvitationEntity>();

    private EventEntity(Event @event)
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
        Author = @event.Author;
        PreviewUrl = @event.PreviewUrl;

        Invitations = @event.Invitations.Select(InvitationEntity.ToEntity).ToList();
    }

    internal EventEntity() { }

    public Event ToDomain()
    {
        return Event.Restore(
            EventId,
            Title,
            ShortDescription,
            Description,
            DateStart,
            LocationId,
            GenreId,
            TypeId,
            MinAge,
            Author,
            PreviewUrl
        );
    }

    public static EventEntity ToEntity(Event domain)
    {
        return new EventEntity(domain);
    }
}
