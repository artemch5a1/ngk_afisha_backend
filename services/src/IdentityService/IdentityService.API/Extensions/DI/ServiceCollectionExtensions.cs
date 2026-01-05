using IdentityService.API.BackgroundServices.Startup;

namespace IdentityService.API.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<StartupService>();
        
        return serviceCollection;
    }
}