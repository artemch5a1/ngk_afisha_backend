using IdentityService.Domain.Abstractions.Infrastructure.Data;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Implementations.Data.Seeding.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Services;

public class PostSeeder : ISeedService
{
    public int Order => 3;

    private readonly IdentityServiceDbContext _db;

    private readonly List<PostSeed> _postSeed;

    public PostSeeder(IdentityServiceDbContext db, IOptions<PostSeedOptions> options)
    {
        _db = db;
        _postSeed = options.Value.PostSeed;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Posts.AnyAsync(cancellationToken))
            return;

        foreach (var postSeed in _postSeed)
        {
            DepartmentEntity department = await _db.Departments.FirstAsync(
                x => x.Title == postSeed.DepartmentName,
                cancellationToken
            );

            await _db.Posts.AddAsync(
                new PostEntity()
                {
                    Title = postSeed.PostName,
                    DepartmentId = department.DepartmentId,
                },
                cancellationToken
            );
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
