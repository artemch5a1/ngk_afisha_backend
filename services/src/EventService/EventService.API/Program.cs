using EventService.API.Extensions.DI;
using EventService.Application.Extensions.DI;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Extensions.DI;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("SeedData.json", optional: false, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
    .Services.AddBackgroundServices()
    .AddRepositories()
    .AddDataService()
    .AddAppServices()
    .AddMediatr(builder.Configuration)
    .AddEntityMappers()
    .ConfigureJwtAuthentication(builder.Configuration)
    .AddAuthorizePolices()
    .AddTransactions()
    .AddTransportService(builder.Configuration)
    .AddApplicationDbContext(
        builder.Configuration.GetConnectionString(nameof(EventServiceDbContext)),
        builder.Configuration
    );

var app = builder.Build();

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
