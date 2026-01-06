using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.LocationCases.DeleteLocation;

public class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand, Result<int>>
{
    private readonly ILocationService _locationService;

    private readonly ILogger<DeleteLocationHandler> _logger;


    public DeleteLocationHandler(ILocationService locationService, ILogger<DeleteLocationHandler> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = 
                await _locationService.DeleteLocation(request.LocationId, cancellationToken);

            return result
                ? Result<int>.Success(request.LocationId)
                : Result<int>.Failure(["Ошибка при обновлении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении локации");

            return Result<int>.Failure(ex);
        }
    }
}