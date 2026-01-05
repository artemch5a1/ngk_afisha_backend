namespace EventService.API.Contracts.Locations;

public class UpdateLocationDto
{
    public int LocationId { get; set; }

    public string Title { get; set; } = null!;

    public string Address { get; set; } = null!;
}