using EventService.Domain.Abstractions.Application.Services.StartupService.Data;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Data.Services.Seeding.Models;
using EventService.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventService.Infrastructure.Data.Services.Seeding.Services;

public class EventTypeSeeder : ISeedService
{
    public int Order => 2;
    
    private readonly EventServiceDbContext _db;
    private readonly List<EventTypeSeed> _eventTypeSeeds;

    public EventTypeSeeder(EventServiceDbContext db, IOptions<EventTypeSeedOptions> options)
    {
        _db = db;
        _eventTypeSeeds = options.Value.EventTypeSeed;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if(await _db.EventTypes.AnyAsync(cancellationToken))
            return;

        foreach (var seed in _eventTypeSeeds)
        {
            await _db.EventTypes.AddAsync(new EventTypeEntity()
            {
                Title = seed.Title
            }, cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}