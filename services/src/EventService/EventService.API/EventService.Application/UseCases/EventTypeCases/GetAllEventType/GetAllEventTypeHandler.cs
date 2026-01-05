using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventTypeCases.GetAllEventType;

public class GetAllEventTypeHandler : IRequestHandler<GetAllEventTypeQuery, Result<List<EventType>>>
{
    private readonly IEventTypeService _eventTypeService;

    private readonly ILogger<GetAllEventTypeHandler> _logger;


    public GetAllEventTypeHandler(IEventTypeService eventTypeService, ILogger<GetAllEventTypeHandler> logger)
    {
        _eventTypeService = eventTypeService;
        _logger = logger;
    }

    public async Task<Result<List<EventType>>> Handle(GetAllEventTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<EventType> location = 
                await _eventTypeService.GetAllEventType(request.Contract, cancellationToken);

            return Result<List<EventType>>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,"Ошбика при получении всех типов событий");

            return Result<List<EventType>>.Failure(ex);
        }
    }
}