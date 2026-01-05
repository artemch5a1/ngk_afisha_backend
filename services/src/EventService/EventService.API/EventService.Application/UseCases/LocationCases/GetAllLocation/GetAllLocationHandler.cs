using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.LocationCases.GetAllLocation;

public class GetAllLocationHandler : IRequestHandler<GetAllLocationQuery, Result<List<Location>>>
{
    private readonly ILocationService _locationService;

    private readonly ILogger<GetAllLocationHandler> _logger;

    public GetAllLocationHandler(ILocationService locationService, ILogger<GetAllLocationHandler> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    public async Task<Result<List<Location>>> Handle(GetAllLocationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Location> location = 
                await _locationService.GetAllLocation(request.Contract, cancellationToken);

            return Result<List<Location>>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,"Ошбика при получении всех локаций");

            return Result<List<Location>>.Failure(ex);
        }
    }
}