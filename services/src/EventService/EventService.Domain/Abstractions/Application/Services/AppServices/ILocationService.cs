using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface ILocationService
{
    Task<List<Location>> GetAllLocation(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<Location?> GetLocationById(int id, CancellationToken cancellationToken = default);

    Task<Location> CreateLocation(
        string title,
        string address,
        CancellationToken cancellationToken = default
    );

    Task<bool> UpdateLocation(
        int locationId,
        string title,
        string address,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteLocation(int locationId, CancellationToken cancellationToken = default);
}
