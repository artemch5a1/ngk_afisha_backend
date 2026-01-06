using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventTypeCases.CreateEventType;

public class CreateEventTypeHandler : IRequestHandler<CreateEventTypeCommand, Result<EventType>>
{
    private readonly IEventTypeService _eventTypeService;

    private readonly ILogger<CreateEventTypeHandler> _logger;


    public CreateEventTypeHandler(IEventTypeService eventTypeService, ILogger<CreateEventTypeHandler> logger)
    {
        _eventTypeService = eventTypeService;
        _logger = logger;
    }

    public async Task<Result<EventType>> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            EventType location = await _eventTypeService.CreateEventType(request.Title, cancellationToken);

            return Result<EventType>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании типа события");

            return Result<EventType>.Failure(ex);
        }
    }
}