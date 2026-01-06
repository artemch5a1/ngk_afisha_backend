namespace EventService.API.Contracts.Events;

public class UpdateEventDto
{
    public Guid EventId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateStart { get; set; }
    
    public int LocationId { get; set; }
    
    public int GenreId { get; set; }
    
    public int TypeId { get; set; }

    public int MinAge { get; set; }
}