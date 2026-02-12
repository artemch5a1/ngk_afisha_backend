namespace EventService.Domain.Abstractions.Application.Services.StartupService.Data;

public interface ISeedService
{
    int Order { get; }

    Task SeedAsync(CancellationToken cancellationToken = default);
}
