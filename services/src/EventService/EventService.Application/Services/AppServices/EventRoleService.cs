using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class EventRoleService : IEventRoleService
{
    private readonly IEventRoleRepository _eventRoleRepository;

    public EventRoleService(IEventRoleRepository eventRoleRepository)
    {
        _eventRoleRepository = eventRoleRepository;
    }

    public async Task<List<EventRole>> GetAllEventRoles(PaginationContract? contract, CancellationToken cancellationToken = default)
    {
        return await _eventRoleRepository.GetAll(contract, cancellationToken);
    }

    public async Task<EventRole?> GetEventRoleById(int eventRoleId, CancellationToken cancellationToken = default)
    {
        return await _eventRoleRepository.GetById(eventRoleId, cancellationToken);
    }

    public async Task<EventRole> CreateEventRole(string title, string description, CancellationToken cancellationToken = default)
    {
        EventRole eventRole = EventRole.Create(title, description);

        return await _eventRoleRepository.Create(eventRole, cancellationToken);
    }

    public async Task<bool> UpdateEventRole(int eventRoleId, string title, string description, CancellationToken cancellationToken = default)
    {
        EventRole? eventRole = await _eventRoleRepository.FindAsync(eventRoleId, cancellationToken);

        if (eventRole is null)
            throw new NotFoundException("Роль события", eventRoleId);

        eventRole.UpdateRole(title, description);

        return await _eventRoleRepository.Update(eventRole, cancellationToken);
    }

    public async Task<bool> DeleteEventRole(int eventRoleId, CancellationToken cancellationToken = default)
    {
        return await _eventRoleRepository.Delete(eventRoleId, cancellationToken);
    }
}