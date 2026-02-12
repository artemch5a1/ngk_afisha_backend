using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.LocationCases.UpdateLocation;

public class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand, Result<int>>
{
    private readonly ILocationService _locationService;

    private readonly ILogger<UpdateLocationHandler> _logger;

    public UpdateLocationHandler(
        ILocationService locationService,
        ILogger<UpdateLocationHandler> logger
    )
    {
        _locationService = locationService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        UpdateLocationCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _locationService.UpdateLocation(
                request.LocationId,
                request.Title,
                request.Address,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.LocationId)
                : Result<int>.Failure(["Ошибка при обновлении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении локации");

            return Result<int>.Failure(ex);
        }
    }
}
