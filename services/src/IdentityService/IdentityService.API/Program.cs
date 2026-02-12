using IdentityService.API.Extensions.DI;
using IdentityService.Application.Extensions.DI;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Extensions.DI;
using IdentityService.Infrastructure.Implementations.Data.Database;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("SeedData.json", optional: false, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // нужно для Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "'Bearer {}' .",
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

builder
    .Services.AddServices()
    .AddMediatr()
    .AddRepositories()
    .AddDataService()
    .AddEntityMappers()
    .AddProviders(builder.Configuration)
    .ConfigureJwtAuthentication(builder.Configuration)
    .AddAuthorizePolices()
    .AddUtils(builder.Configuration)
    .AddTransactions()
    .AddBackgroundServices()
    .AddApplicationDbContext(
        builder.Configuration.GetConnectionString(nameof(IdentityServiceDbContext)),
        builder.Configuration
    );

WebApplication app = builder.Build();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    }
);

app.MapFallback("/error", () => Results.Problem());
app.UseExceptionHandler("/error");

bool includeDocumentationInRelease = app.Configuration.GetValue<bool>(
    "IncludeDocumentationInRelease"
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || includeDocumentationInRelease)
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty; // Swagger доступен по адресу /
    });
    app.MapGet("/", () => Results.Redirect("/swagger/index.html")).ExcludeFromDescription();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins().AllowAnyOrigin();
    x.WithMethods().AllowAnyMethod();
});

app.Run();
