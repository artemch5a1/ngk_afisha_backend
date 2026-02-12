using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.LocationCases.GetLocationById;

public class GetLocationByIdHandler : IRequestHandler<GetLocationByIdQuery, Result<Location>>
{
    private readonly ILocationService _locationService;

    private readonly ILogger<GetLocationByIdHandler> _logger;

    public GetLocationByIdHandler(
        ILocationService locationService,
        ILogger<GetLocationByIdHandler> logger
    )
    {
        _locationService = locationService;
        _logger = logger;
    }

    public async Task<Result<Location>> Handle(
        GetLocationByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Location? location = await _locationService.GetLocationById(
                request.LocationId,
                cancellationToken
            );

            if (location is null)
                return Result<Location>.Failure(["Локация не найдена"], ApiErrorType.NotFound);

            return Result<Location>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локации по id");

            return Result<Location>.Failure(ex);
        }
    }
}
