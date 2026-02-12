using EventService.Domain.Abstractions.Application.Services.StartupService.Data;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Data.Services.Seeding.Models;
using EventService.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventService.Infrastructure.Data.Services.Seeding.Services;

public class EventRoleSeeder : ISeedService
{
    public int Order => 1;

    private readonly EventServiceDbContext _db;
    private readonly List<EventRoleSeed> _eventRoleSeeds;

    public EventRoleSeeder(EventServiceDbContext db, IOptions<EventRoleSeedOptions> options)
    {
        _db = db;
        _eventRoleSeeds = options.Value.EventRoleSeed;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.EventRoles.AnyAsync(cancellationToken))
            return;

        foreach (var seed in _eventRoleSeeds)
        {
            await _db.EventRoles.AddAsync(
                new EventRoleEntity() { Title = seed.Title, Description = seed.Description },
                cancellationToken
            );
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
