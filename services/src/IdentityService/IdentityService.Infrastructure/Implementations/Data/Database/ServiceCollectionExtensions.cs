using IdentityService.Domain.Abstractions.Application.Services.StartupService;
using IdentityService.Domain.Abstractions.Infrastructure.Data;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Implementations.Data.Seeding;
using IdentityService.Infrastructure.Implementations.Data.Seeding.Models;
using IdentityService.Infrastructure.Implementations.Data.Seeding.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Implementations.Data.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDbContext(
        this IServiceCollection serviceCollection,
        string? connectString,
        IConfiguration configuration
    )
    {
        serviceCollection.AddDbContext<IdentityServiceDbContext>(options =>
            options.UseNpgsql(connectString)
        );

        serviceCollection.AddScoped<IStartupService, DatabaseSeeder>();

        serviceCollection.Configure<SpecialtySeedOption>(configuration.GetSection("SeedData"));

        serviceCollection.AddScoped<ISeedService, SpecialtySeeder>();

        serviceCollection.Configure<GroupsSeedOption>(configuration.GetSection("SeedData"));

        serviceCollection.AddScoped<ISeedService, GroupsSeeder>();

        serviceCollection.Configure<DepartmentSeedOptions>(configuration.GetSection("SeedData"));

        serviceCollection.AddScoped<ISeedService, DepartmentSeeder>();

        serviceCollection.Configure<PostSeedOptions>(configuration.GetSection("SeedData"));

        serviceCollection.AddScoped<ISeedService, PostSeeder>();

        return serviceCollection;
    }
}
