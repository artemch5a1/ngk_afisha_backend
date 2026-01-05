using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class EventTypeService : IEventTypeService
{
    private readonly IEventTypeRepository _eventTypeRepository;

    public EventTypeService(IEventTypeRepository eventTypeRepository)
    {
        _eventTypeRepository = eventTypeRepository;
    }

    public async Task<List<EventType>> GetAllEventType(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        return await _eventTypeRepository.GetAll(contract, cancellationToken);
    }

    public async Task<EventType?> GetEventTypeById(int typeId, CancellationToken cancellationToken = default)
    {
        return await _eventTypeRepository.GetById(typeId, cancellationToken);
    }

    public async Task<EventType> CreateEventType(string title, CancellationToken cancellationToken = default)
    {
        EventType eventType = EventType.Create(title);
        
        return await _eventTypeRepository.Create(eventType, cancellationToken);
    }

    public async Task<bool> UpdateEventType(int typeId, string title, CancellationToken cancellationToken = default)
    {
        EventType? eventType = await _eventTypeRepository.FindAsync(typeId, cancellationToken);

        if (eventType is null)
            throw new NotFoundException("Тип события", typeId);
        
        eventType.UpdateEventType(title);

        return await _eventTypeRepository.Update(eventType, cancellationToken);
    }

    public async Task<bool> DeleteEventType(int typeId, CancellationToken cancellationToken = default)
    {
        return await _eventTypeRepository.Delete(typeId, cancellationToken);
    }
}