using EventService.API.Contracts.Events;
using EventService.Application.Contract;
using EventService.Application.UseCases.EventCases.CreateEvent;
using EventService.Domain.Models;
using UpdateEventCommand = EventService.Application.UseCases.EventCases.UpdateEvent.UpdateEventCommand;

namespace EventService.API.Extensions.Mappings;

public static class EventMapper
{
    public static UpdatedEventDto ToDto(this UpdatedEvent @event)
    {
        return new UpdatedEventDto()
        {
            EventId = @event.EventId,
            UploadUrl = @event.UploadUrl
        };
    }

    public static CreatedEventDto ToDto(this CreatedEvent @event)
    {
        CreatedEventDto eventDto = new CreatedEventDto()
        {
            EventId = @event.EventId,
            Title = @event.Title,
            ShortDescription = @event.ShortDescription,
            Description = @event.Description,
            DateStart = @event.DateStart,
            LocationId = @event.LocationId,
            GenreId = @event.GenreId,
            TypeId = @event.TypeId,
            MinAge = @event.MinAge,
            UploadUrl = @event.UploadUrl
        };

        if (@event.Location is not null)
            eventDto.Location = @event.Location.ToDto();

        if (@event.Genre is not null)
            eventDto.Genre = @event.Genre.ToDto();
        
        if(@event.Type is not null)
            eventDto.Type = @event.Type.ToDto();
        
        return eventDto;
    }

    public static EventDto ToDto(this Event @event)
    {
        EventDto eventDto = new EventDto()
        {
            EventId = @event.EventId,
            Title = @event.Title,
            ShortDescription = @event.ShortDescription,
            Description = @event.Description,
            DateStart = @event.DateStart,
            LocationId = @event.LocationId,
            GenreId = @event.GenreId,
            TypeId = @event.TypeId,
            MinAge = @event.MinAge,
            DownloadUrl = @event.DownloadUrl
        };

        if (@event.Location is not null)
            eventDto.Location = @event.Location.ToDto();

        if (@event.Genre is not null)
            eventDto.Genre = @event.Genre.ToDto();
        
        if(@event.Type is not null)
            eventDto.Type = @event.Type.ToDto();
        
        return eventDto;
    }

    public static List<EventDto> ToListDto(this List<Event> events) =>
        events.Select(x => x.ToDto()).ToList();

    public static CreateEventCommand ToCommand(this CreateEventDto dto, Guid authorId)
    {
        return new CreateEventCommand(
            dto.Title, 
            dto.ShortDescription, 
            dto.Description, 
            dto.DateStart,
            dto.LocationId,
            dto.GenreId,
            dto.TypeId,
            dto.MinAge, 
            authorId);
    }
    
    public static UpdateEventCommand ToCommand(this UpdateEventDto dto, Guid currentUser)
    {
        return new UpdateEventCommand(
            currentUser,
            dto.EventId,
            dto.Title, 
            dto.ShortDescription, 
            dto.Description, 
            dto.DateStart, 
            dto.LocationId,
            dto.GenreId,
            dto.TypeId,
            dto.MinAge);
    }
}