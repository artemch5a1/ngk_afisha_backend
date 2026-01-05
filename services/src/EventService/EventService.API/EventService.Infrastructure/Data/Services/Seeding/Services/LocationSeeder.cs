using EventService.Domain.Abstractions.Application.Services.StartupService.Data;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Data.Services.Seeding.Models;
using EventService.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventService.Infrastructure.Data.Services.Seeding.Services;

public class LocationSeeder : ISeedService
{
    public int Order => 4;
    
    private readonly EventServiceDbContext _db;
    
    private readonly List<LocationSeed> _locationSeeds;
    
    public LocationSeeder(EventServiceDbContext db, IOptions<LocationSeedOptions> options)
    {
        _db = db;
        _locationSeeds = options.Value.LocationSeed;
    }
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if(await _db.Locations.AnyAsync(cancellationToken))
            return;

        foreach (var seed in _locationSeeds)
        {
            await _db.Locations.AddAsync(new LocationEntity()
            {
                Title = seed.Title,
                Address = seed.Address,
            }, cancellationToken);
        }
        
        await _db.SaveChangesAsync(cancellationToken);
    }
}