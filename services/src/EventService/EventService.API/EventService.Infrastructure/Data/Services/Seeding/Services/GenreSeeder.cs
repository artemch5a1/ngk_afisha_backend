using EventService.Domain.Abstractions.Application.Services.StartupService.Data;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Data.Services.Seeding.Models;
using EventService.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventService.Infrastructure.Data.Services.Seeding.Services;

public class GenreSeeder : ISeedService
{
    public int Order => 0;
    
    private readonly EventServiceDbContext _db;
    
    private readonly List<GenreSeed> _genreSeeds;
    
    public GenreSeeder(EventServiceDbContext db, IOptions<GenreSeedOptions> options)
    {
        _db = db;
        _genreSeeds = options.Value.GenreSeed;
    }
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if(await _db.Genres.AnyAsync(cancellationToken))
            return;

        foreach (GenreSeed genreSeed in _genreSeeds)
        {
            await _db.Genres.AddAsync(new GenreEntity()
            {
                Title = genreSeed.GenreName
            } ,cancellationToken);
        }
        
        await _db.SaveChangesAsync(cancellationToken);
    }
}