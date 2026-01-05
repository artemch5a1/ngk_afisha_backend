using IdentityService.Domain.Abstractions.Infrastructure.Data;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Implementations.Data.Seeding.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Services;

public class DepartmentSeeder : ISeedService
{
    public int Order => 2;
    
    private readonly IdentityServiceDbContext _db;

    private readonly List<DepartmentSeed> _departmentSeed;
    
    public DepartmentSeeder(
        IdentityServiceDbContext db, 
        IOptions<DepartmentSeedOptions> options)
    {
        _db = db;
        _departmentSeed = options.Value.DepartmentSeed;
    }
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Departments.AnyAsync(cancellationToken))
            return;

        foreach (var departmentSeed in _departmentSeed)
        {
            await _db.Departments.AddAsync(new DepartmentEntity()
                {
                    Title = departmentSeed.DepartmentName
                }, cancellationToken
            );
        }
        
        await _db.SaveChangesAsync(cancellationToken);
    }
}