using IdentityService.Domain.Abstractions.Application.Services.StartupService;
using IdentityService.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data.Services.Migrations;

public class MigrationService(IdentityServiceDbContext db) : IStartupService
{
    public int Order => 0;

    public async Task InvokeAsync(CancellationToken ct = default) =>
        await db.Database.MigrateAsync(ct);
}
