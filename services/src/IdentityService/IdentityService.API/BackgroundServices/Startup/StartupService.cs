using IdentityService.Domain.Abstractions.Application.Services.StartupService;

namespace IdentityService.API.BackgroundServices.Startup;

public class StartupService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StartupService> _logger;

    public StartupService(IServiceProvider serviceProvider, ILogger<StartupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Выполняется стартовая инициализация...");

        using var scope = _serviceProvider.CreateScope();

        var startupServices = scope
            .ServiceProvider.GetRequiredService<IEnumerable<IStartupService>>()
            .OrderBy(s => s.Order);

        foreach (var service in startupServices)
        {
            _logger.LogInformation("Выполняется {Service}", service.GetType().Name);
            await service.InvokeAsync(cancellationToken);
        }

        _logger.LogInformation("Инициализация завершена");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
