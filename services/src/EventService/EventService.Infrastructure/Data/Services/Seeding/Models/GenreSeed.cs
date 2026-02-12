namespace EventService.Infrastructure.Data.Services.Seeding.Models;

public class GenreSeed
{
    public string GenreName { get; set; } = null!;
}

public class GenreSeedOptions
{
    public List<GenreSeed> GenreSeed { get; set; } = [];
}
