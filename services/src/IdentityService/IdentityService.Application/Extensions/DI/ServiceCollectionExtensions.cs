using IdentityService.Application.PipelineBehaviors;
using IdentityService.Application.Services.AccountContext;
using IdentityService.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountService>();

        serviceCollection.AddScoped<IUserService, UserService>();

        serviceCollection.AddScoped<ISpecialtyService, SpecialtyService>();

        serviceCollection.AddScoped<IGroupService, GroupService>();

        serviceCollection.AddScoped<IStudentService, StudentService>();

        serviceCollection.AddScoped<IDepartmentService, DepartmentService>();

        serviceCollection.AddScoped<IPostService, PostService>();

        serviceCollection.AddScoped<IPublisherService, PublisherService>();

        return serviceCollection;
    }

    public static IServiceCollection AddMediatr(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly)
        );

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return serviceCollection;
    }
}
