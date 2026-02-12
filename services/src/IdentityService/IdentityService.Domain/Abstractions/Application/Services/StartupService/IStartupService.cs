namespace IdentityService.Domain.Abstractions.Application.Services.StartupService;

public interface IStartupService
{
    int Order { get; }

    Task InvokeAsync(CancellationToken ct = default);
}
