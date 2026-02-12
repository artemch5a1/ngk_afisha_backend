using IdentityService.Domain.Abstractions.Infrastructure.Data;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Implementations.Data.Seeding.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Services;

public class GroupsSeeder : ISeedService
{
    public int Order => 1;
    private readonly IdentityServiceDbContext _db;

    private readonly List<GroupsSeed> _groupSeeds;

    public GroupsSeeder(IdentityServiceDbContext db, IOptions<GroupsSeedOption> options)
    {
        _db = db;
        _groupSeeds = options.Value.GroupSeed;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Groups.AnyAsync(cancellationToken))
            return;

        foreach (GroupsSeed group in _groupSeeds)
        {
            SpecialtyEntity specialty = await _db.Specialties.FirstAsync(
                x => x.SpecialtyTitle == group.SpecialtyTitle,
                cancellationToken
            );

            await _db.Groups.AddAsync(
                new GroupEntity()
                {
                    Course = group.Course,
                    NumberGroup = group.Number,
                    SpecialtyId = specialty.SpecialtyId,
                },
                cancellationToken
            );
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
