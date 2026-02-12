using IdentityService.Domain.Abstractions.Infrastructure.Data;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Implementations.Data.Seeding.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Services;

public class SpecialtySeeder : ISeedService
{
    private readonly IdentityServiceDbContext _db;

    private readonly List<SpecialtySeed> _specialtySeeds;

    public SpecialtySeeder(IdentityServiceDbContext db, IOptions<SpecialtySeedOption> options)
    {
        _db = db;
        _specialtySeeds = options.Value.SpecialtySeed;
    }

    public int Order => 0;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Specialties.AnyAsync(cancellationToken))
            return;

        foreach (SpecialtySeed specialty in _specialtySeeds)
        {
            await _db.Specialties.AddAsync(
                new SpecialtyEntity() { SpecialtyTitle = specialty.SpecialtyTitle },
                cancellationToken
            );
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
