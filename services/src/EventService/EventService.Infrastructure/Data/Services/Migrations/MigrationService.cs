using EventService.Domain.Abstractions.Application.Services.StartupService;
using EventService.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Data.Services.Migrations;

public class MigrationService(EventServiceDbContext db) : IStartupService
{
    public int Order => 0;

    public async Task InvokeAsync(CancellationToken ct = default) =>
        await db.Database.MigrateAsync(ct);
}
