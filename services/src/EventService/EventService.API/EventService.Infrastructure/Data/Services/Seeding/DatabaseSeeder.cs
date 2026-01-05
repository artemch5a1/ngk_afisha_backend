using EventService.Domain.Abstractions.Application.Services.StartupService;
using EventService.Domain.Abstractions.Application.Services.StartupService.Data;
using Microsoft.Extensions.DependencyInjection;

namespace EventService.Infrastructure.Data.Services.Seeding;

public class DatabaseSeeder : IStartupService
{
    public int Order => 1;
    
    private readonly IServiceProvider _serviceProvider;

    public DatabaseSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(CancellationToken ct = default)
    {
        IEnumerable<ISeedService> seedServices = _serviceProvider
            .GetServices<ISeedService>()
            .OrderBy(x => x.Order);

        foreach (ISeedService seedService in seedServices)
        {
            await seedService.SeedAsync(ct);
        }
    }
}