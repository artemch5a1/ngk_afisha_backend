using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventTypeCases.GetEventTypeById;

public class GetEventTypeByIdHandler : IRequestHandler<GetEventTypeByIdQuery, Result<EventType>>
{
    private readonly IEventTypeService _eventTypeService;

    private readonly ILogger<GetEventTypeByIdHandler> _logger;

    public GetEventTypeByIdHandler(
        IEventTypeService eventTypeService,
        ILogger<GetEventTypeByIdHandler> logger
    )
    {
        _eventTypeService = eventTypeService;
        _logger = logger;
    }

    public async Task<Result<EventType>> Handle(
        GetEventTypeByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            EventType? location = await _eventTypeService.GetEventTypeById(
                request.EventTypeId,
                cancellationToken
            );

            if (location is null)
                return Result<EventType>.Failure(["Тип события не найден"], ApiErrorType.NotFound);

            return Result<EventType>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении типа события по id");

            return Result<EventType>.Failure(ex);
        }
    }
}
