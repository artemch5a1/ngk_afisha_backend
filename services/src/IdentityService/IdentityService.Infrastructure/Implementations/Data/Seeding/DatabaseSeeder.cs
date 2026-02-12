using IdentityService.Domain.Abstractions.Application.Services.StartupService;
using IdentityService.Domain.Abstractions.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Implementations.Data.Seeding;

public class DatabaseSeeder : IStartupService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public int Order => 2;

    public async Task InvokeAsync(CancellationToken ct = default)
    {
        IEnumerable<ISeedService> seedServices = _serviceProvider
            .GetServices<ISeedService>()
            .OrderBy(s => s.Order);

        foreach (var seedService in seedServices)
        {
            await seedService.SeedAsync(ct);
        }
    }
}
