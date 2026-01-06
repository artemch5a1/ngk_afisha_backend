using System.Security.Claims;
using System.Security.Cryptography;
using EventService.Domain.Abstractions.Application.Services.StartupService;
using EventService.Domain.Abstractions.Application.Services.StartupService.Data;
using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Abstractions.Infrastructure.Transactions;
using EventService.Domain.Enums;
using EventService.Domain.Extensions;
using EventService.Domain.Models;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Data.Services.Migrations;
using EventService.Infrastructure.Data.Services.Seeding;
using EventService.Infrastructure.Data.Services.Seeding.Models;
using EventService.Infrastructure.Data.Services.Seeding.Services;
using EventService.Infrastructure.Entites;
using EventService.Infrastructure.Implementations.Mapping;
using EventService.Infrastructure.Implementations.Mapping.Base;
using EventService.Infrastructure.Implementations.Repositories;
using EventService.Infrastructure.Implementations.SpecificationsImpl;
using EventService.Infrastructure.Implementations.SpecificationsImpl.Impls.Events;
using EventService.Infrastructure.Implementations.Storage;
using EventService.Infrastructure.Implementations.Transactions;
using EventService.Infrastructure.Static;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EventService.Infrastructure.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDbContext(
        this IServiceCollection serviceCollection, 
        string? connectString,
        IConfiguration configuration
        )
    {
        serviceCollection.AddDbContext<EventServiceDbContext>(options =>
            options.UseNpgsql(connectString)
        );

        serviceCollection.AddScoped<IStartupService, DatabaseSeeder>();

        serviceCollection
            .Configure<GenreSeedOptions>(configuration.GetSection("SeedData"));
        
        serviceCollection.AddScoped<ISeedService, GenreSeeder>();
        
        serviceCollection.Configure<EventRoleSeedOptions>(configuration.GetSection("SeedData"));
        serviceCollection.AddScoped<ISeedService, EventRoleSeeder>();
        
        serviceCollection.Configure<EventTypeSeedOptions>(configuration.GetSection("SeedData"));
        serviceCollection.AddScoped<ISeedService, EventTypeSeeder>();
        
        serviceCollection.Configure<LocationSeedOptions>(configuration.GetSection("SeedData"));
        serviceCollection.AddScoped<ISeedService, LocationSeeder>();

        AddSpecifications(serviceCollection);
        
        return serviceCollection;
    }

    private static void AddSpecifications(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<EfSpecificationMapper>();
        
        serviceCollection.AddSingleton<IEfSpecificationHandler<Event, EventEntity>, UpcomingEventsSpecification>();
    }

    public static IServiceCollection AddTransportService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<S3StorageSettings>(
            opt =>
            {
                opt.BucketName = configuration["S3Storage:BucketName"]!;
                opt.AccessKey = configuration["S3Storage:AccessKey"]!;
                opt.SecretKey = configuration["S3Storage:SecretKey"]!;
                opt.ServiceUrl = configuration["S3Storage:ServiceUrl"]!;
                opt.ServiceUrlBounded = configuration.GetValue<string?>("S3Storage:ServiceUrlBounded");
            });

        serviceCollection.AddScoped<IStorageService, S3StorageService>();

        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEventRepository, EventRepository>();

        serviceCollection.AddScoped<ILocationRepository, LocationRepository>();

        serviceCollection.AddScoped<IGenreRepository, GenreRepository>();

        serviceCollection.AddScoped<IEventTypeRepository, EventTypeRepository>();

        serviceCollection.AddScoped<IInvitationRepository, InvitationRepository>();

        serviceCollection.AddScoped<IEventRoleRepository, EventRoleRepository>();

        serviceCollection.AddScoped<IMemberRepository, MemberRepository>();
        
        return serviceCollection;
    }

    public static IServiceCollection AddTransactions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        return serviceCollection;
    }
    
    public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rsa = RSA.Create();

        string publicKey = configuration["Jwt:PublicKey"]!;
        
        var keyBytes = Convert.FromBase64String(publicKey
            .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
            .Replace("-----END RSA PRIVATE KEY-----", "")
            .Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Trim());
        
        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
        
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(
                            rsa
                        ),
                        RoleClaimType = ClaimTypes.Role,
                    };
                }
            );

        return serviceCollection;
    }

    public static IServiceCollection AddAuthorizePolices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthorizationBuilder()
            .AddPolicy(
                PolicyNames.AdminOnly,
                policy => policy.RequireRole(Role.Admin.GetString())
            )
            .AddPolicy(
                PolicyNames.UserOrAdmin,
                policy =>
                    policy.RequireRole(Role.User.GetString(), Role.Admin.GetString())
            )
            .AddPolicy(
                PolicyNames.PublisherOrAdmin,
                policy =>
                    policy.RequireRole(Role.Publisher.GetString(), Role.Admin.GetString())
            )
            .AddPolicy(
                PolicyNames.PublisherOnly,
                policy => policy.RequireRole(Role.Publisher.GetString())
            )
            .AddPolicy(
                PolicyNames.UserOnly,
                policy => policy.RequireRole(Role.User.GetString())
            );

        return serviceCollection;
    }
    
    public static IServiceCollection AddEntityMappers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IEntityMapper<,>), typeof(BaseEntityMapper<,>));

        serviceCollection.AddScoped<IEntityMapper<EventEntity, Event>, EventMapper>();

        serviceCollection.AddScoped<IEntityMapper<InvitationEntity, Invitation>, InvitationMapper>();

        serviceCollection.AddScoped<IEntityMapper<MemberEntity, Member>, MemberMapper>();
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddDataService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IStartupService, MigrationService>();

        return serviceCollection;
    }
}