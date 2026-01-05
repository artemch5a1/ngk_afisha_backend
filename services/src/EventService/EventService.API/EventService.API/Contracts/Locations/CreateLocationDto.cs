namespace EventService.API.Contracts.Locations;

public class CreateLocationDto
{
    public string Title { get; set; } = null!;

    public string Address { get; set; } = null!;
}