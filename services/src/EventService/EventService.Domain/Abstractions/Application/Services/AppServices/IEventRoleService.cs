using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface IEventRoleService
{
    Task<List<EventRole>> GetAllEventRoles(
        PaginationContract? contract,
        CancellationToken cancellationToken = default
    );

    Task<EventRole?> GetEventRoleById(
        int eventRoleId,
        CancellationToken cancellationToken = default
    );

    Task<EventRole> CreateEventRole(
        string title,
        string description,
        CancellationToken cancellationToken = default
    );

    Task<bool> UpdateEventRole(
        int eventRoleId,
        string title,
        string description,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteEventRole(int eventRoleId, CancellationToken cancellationToken = default);
}
