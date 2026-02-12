namespace EventService.Infrastructure.Data.Services.Seeding.Models;

public class LocationSeed
{
    public string Title { get; set; } = null!;

    public string Address { get; set; } = null!;
}

public class LocationSeedOptions
{
    public List<LocationSeed> LocationSeed { get; set; } = [];
}
