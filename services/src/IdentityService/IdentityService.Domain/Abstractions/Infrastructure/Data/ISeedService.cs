namespace IdentityService.Domain.Abstractions.Infrastructure.Data;

public interface ISeedService
{
    int Order { get; }
    
    Task SeedAsync(CancellationToken cancellationToken = default);
}