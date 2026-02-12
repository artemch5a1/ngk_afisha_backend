using System.Security.Claims;
using System.Security.Cryptography;
using IdentityService.Domain.Abstractions.Application.Services.StartupService;
using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Abstractions.Infrastructure.Providers;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.AccountContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Transactions;
using IdentityService.Domain.Abstractions.Infrastructure.Utils;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Extensions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Data.Services.Migrations;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Implementations.Mapping;
using IdentityService.Infrastructure.Implementations.Mapping.Base;
using IdentityService.Infrastructure.Implementations.Providers.AccessTokenProvider;
using IdentityService.Infrastructure.Implementations.Repositories.AccountContext;
using IdentityService.Infrastructure.Implementations.Repositories.UserContext;
using IdentityService.Infrastructure.Implementations.Transactions;
using IdentityService.Infrastructure.Implementations.Utils;
using IdentityService.Infrastructure.Implementations.Utils.AdminRegistry;
using IdentityService.Infrastructure.Static;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountRepository, AccountRepository>();

        serviceCollection.AddScoped<IUserRepository, UserRepository>();

        serviceCollection.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

        serviceCollection.AddScoped<IGroupRepository, GroupRepository>();

        serviceCollection.AddScoped<IStudentRepository, StudentRepository>();

        serviceCollection.AddScoped<IDepartmentRepository, DepartmentRepository>();

        serviceCollection.AddScoped<IPostRepository, PostRepository>();

        serviceCollection.AddScoped<IPublisherRepository, PublisherRepository>();

        return serviceCollection;
    }

    public static IServiceCollection AddTransactions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        return serviceCollection;
    }

    public static IServiceCollection AddUtils(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        serviceCollection.AddScoped<IPasswordHasher, BCryptHasher>();

        serviceCollection.Configure<AdminCred>(opt =>
        {
            opt.Email = configuration["AdminCred:Email"]!;
            opt.Password = configuration["AdminCred:Password"]!;
        });

        serviceCollection.AddScoped<IStartupService, AdminRegistry>();

        return serviceCollection;
    }

    public static IServiceCollection AddProviders(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        serviceCollection.Configure<JwtOptions>(opt =>
        {
            opt.Issuer = configuration["Jwt:Issuer"]!;
            opt.PrivateKey = configuration["Jwt:PrivateKey"]!;
            opt.PublicKey = configuration["Jwt:PublicKey"]!;
            opt.ExpiresMinutes = int.TryParse(configuration["Jwt:Expires"], out int m) ? m : 120;
        });

        serviceCollection.AddScoped<IAccessTokenProvider, JwtProvider>();

        return serviceCollection;
    }

    public static IServiceCollection ConfigureJwtAuthentication(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        var rsa = RSA.Create();

        string publicKey = configuration["Jwt:PublicKey"]!;

        var keyBytes = Convert.FromBase64String(
            publicKey
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim()
        );

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
                        IssuerSigningKey = new RsaSecurityKey(rsa),
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
            .AddPolicy(PolicyNames.AdminOnly, policy => policy.RequireRole(Role.Admin.GetString()))
            .AddPolicy(
                PolicyNames.UserOrAdmin,
                policy => policy.RequireRole(Role.User.GetString(), Role.Admin.GetString())
            )
            .AddPolicy(
                PolicyNames.PublisherOrAdmin,
                policy => policy.RequireRole(Role.Publisher.GetString(), Role.Admin.GetString())
            )
            .AddPolicy(
                PolicyNames.PublisherOnly,
                policy => policy.RequireRole(Role.Publisher.GetString())
            )
            .AddPolicy(PolicyNames.UserOnly, policy => policy.RequireRole(Role.User.GetString()));

        return serviceCollection;
    }

    public static IServiceCollection AddEntityMappers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IEntityMapper<,>), typeof(BaseEntityMapper<,>));

        serviceCollection.AddScoped<IEntityMapper<GroupEntity, Group>, GroupMapper>();

        serviceCollection.AddScoped<IEntityMapper<UserEntity, User>, UserMapper>();

        serviceCollection.AddScoped<IEntityMapper<StudentEntity, Student>, StudentMapper>();

        serviceCollection.AddScoped<IEntityMapper<PostEntity, Post>, PostMapper>();

        serviceCollection.AddScoped<IEntityMapper<PublisherEntity, Publisher>, PublisherMapper>();

        return serviceCollection;
    }

    public static IServiceCollection AddDataService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IStartupService, MigrationService>();

        return serviceCollection;
    }
}
