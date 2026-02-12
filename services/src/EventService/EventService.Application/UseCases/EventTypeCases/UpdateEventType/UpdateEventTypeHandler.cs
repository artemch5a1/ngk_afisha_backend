using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventTypeCases.UpdateEventType;

public class UpdateEventTypeHandler : IRequestHandler<UpdateEventTypeCommand, Result<int>>
{
    private readonly IEventTypeService _eventTypeService;

    private readonly ILogger<UpdateEventTypeHandler> _logger;

    public UpdateEventTypeHandler(
        IEventTypeService eventTypeService,
        ILogger<UpdateEventTypeHandler> logger
    )
    {
        _eventTypeService = eventTypeService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        UpdateEventTypeCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _eventTypeService.UpdateEventType(
                request.TypeId,
                request.Title,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.TypeId)
                : Result<int>.Failure(["Ошибка при обновлении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении типа события");

            return Result<int>.Failure(ex);
        }
    }
}
