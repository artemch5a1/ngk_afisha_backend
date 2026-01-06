using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface IEventTypeService
{
    Task<List<EventType>> GetAllEventType(PaginationContract? contract = null, CancellationToken cancellationToken = default);

    Task<EventType?> GetEventTypeById(int typeId, CancellationToken cancellationToken = default);

    Task<EventType> CreateEventType(string title,CancellationToken cancellationToken = default);

    Task<bool> UpdateEventType(int typeId, string title, CancellationToken cancellationToken = default);

    Task<bool> DeleteEventType(int typeId, CancellationToken cancellationToken = default);
}