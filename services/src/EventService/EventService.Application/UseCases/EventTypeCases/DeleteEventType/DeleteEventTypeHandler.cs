using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventTypeCases.DeleteEventType;

public class DeleteEventTypeHandler : IRequestHandler<DeleteEventTypeCommand, Result<int>>
{
    private readonly IEventTypeService _eventTypeService;

    private readonly ILogger<DeleteEventTypeHandler> _logger;

    public DeleteEventTypeHandler(
        IEventTypeService eventTypeService,
        ILogger<DeleteEventTypeHandler> logger
    )
    {
        _eventTypeService = eventTypeService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        DeleteEventTypeCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _eventTypeService.DeleteEventType(
                request.TypeId,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.TypeId)
                : Result<int>.Failure(["Ошибка при удалении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении типа события");

            return Result<int>.Failure(ex);
        }
    }
}
