using EventService.API.Contracts.EventRole;
using EventService.Application.UseCases.EventRoleUseCases.CreateEventRole;
using EventService.Application.UseCases.EventRoleUseCases.UpdateEventRole;
using EventService.Domain.Models;

namespace EventService.API.Extensions.Mappings;

public static class EventRoleMapper
{
    public static EventRoleDto ToDto(this EventRole eventRole)
    {
        return new EventRoleDto()
        {
            EventRoleId = eventRole.EventRoleId,
            Title = eventRole.Title,
            Description = eventRole.Description,
        };
    }

    public static List<EventRoleDto> ToListDto(this List<EventRole> eventRoles) =>
        eventRoles.Select(x => x.ToDto()).ToList();

    public static CreateEventRoleCommand ToCommand(this CreateEventRoleDto dto)
    {
        return new CreateEventRoleCommand(dto.Title, dto.Description);
    }

    public static UpdateEventRoleCommand ToCommand(this UpdateEventRoleDto dto)
    {
        return new UpdateEventRoleCommand(dto.EventRoleId, dto.Title, dto.Description);
    }
}
