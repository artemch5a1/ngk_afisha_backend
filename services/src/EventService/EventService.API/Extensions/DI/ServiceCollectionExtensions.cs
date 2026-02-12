using EventService.API.BackgroundServices.Startup;

namespace EventService.API.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection serviceCollection
    )
    {
        serviceCollection.AddHostedService<StartupService>();

        return serviceCollection;
    }
}
