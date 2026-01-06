namespace EventService.API.Contracts.Locations;

public class LocationDto
{
    public int LocationId { get; set; }

    public string Title { get; set; } = null!;

    public string Address { get; set; } = null!;
}