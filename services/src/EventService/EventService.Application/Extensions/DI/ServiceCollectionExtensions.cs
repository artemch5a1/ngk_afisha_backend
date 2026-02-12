using EventService.Application.Contract;
using EventService.Application.PipelineBehaviors;
using EventService.Application.Services.AppServices;
using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventService.Application.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEventService, EventServiceCore>();

        serviceCollection.AddScoped<ILocationService, LocationService>();

        serviceCollection.AddScoped<IGenreService, GenreService>();

        serviceCollection.AddScoped<IEventTypeService, EventTypeService>();

        serviceCollection.AddScoped<IInvitationService, InvitationService>();

        serviceCollection.AddScoped<IEventRoleService, EventRoleService>();

        serviceCollection.AddScoped<IMemberService, MemberService>();

        return serviceCollection;
    }

    public static IServiceCollection AddMediatr(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        serviceCollection.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly)
        );

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        serviceCollection.Configure<EventSetting>(opt =>
        {
            opt.TimeActiveDownloadLinkInMilliSeconds = int.Parse(
                configuration["EventSettings:TimeActiveDownloadLinkInMilliSeconds"]!
            );
            opt.TimeActiveUploadLinkInMilliSeconds = int.Parse(
                configuration["EventSettings:TimeActiveUploadLinkInMilliSeconds"]!
            );
        });

        serviceCollection.Configure<DefaultEventOptions>(configuration.GetSection("SeedData"));

        return serviceCollection;
    }
}
