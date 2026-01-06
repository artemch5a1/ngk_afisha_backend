using EventService.API.Contracts.EventTypes;
using EventService.Application.UseCases.EventTypeCases.CreateEventType;
using EventService.Application.UseCases.EventTypeCases.UpdateEventType;
using EventService.Domain.Models;

namespace EventService.API.Extensions.Mappings;

public static class EventTypeMapper
{
    public static EventTypeDto ToDto(this EventType eventType)
    {
        return new EventTypeDto()
        {
            TypeId = eventType.TypeId,
            Title = eventType.Title
        };
    }

    public static List<EventTypeDto> ToListDto(this List<EventType> eventTypes)
        => eventTypes.Select(x => x.ToDto()).ToList();

    public static UpdateEventTypeCommand ToCommand(this UpdateEventTypeDto dto)
    {
        return new UpdateEventTypeCommand(dto.TypeId, dto.Title);
    }
    
    public static CreateEventTypeCommand ToCommand(this CreateEventTypeDto dto)
    {
        return new CreateEventTypeCommand(dto.Title);
    }
}