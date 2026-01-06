using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.LocationCases.CreateLocation;

public class CreateLocationHandler : IRequestHandler<CreateLocationCommand, Result<Location>>
{
    private readonly ILocationService _locationService;

    private readonly ILogger<CreateLocationHandler> _logger;


    public CreateLocationHandler(ILocationService locationService, ILogger<CreateLocationHandler> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    public async Task<Result<Location>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Location location = await _locationService.CreateLocation(request.Title, request.Address, cancellationToken);

            return Result<Location>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании локации");

            return Result<Location>.Failure(ex);
        }
    }
}