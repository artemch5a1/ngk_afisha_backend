using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class LocationService: ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<List<Location>> GetAllLocation(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        return await _locationRepository.GetAll(contract, cancellationToken);
    }

    public async Task<Location?> GetLocationById(int id, CancellationToken cancellationToken = default)
    {
        return await _locationRepository.GetById(id, cancellationToken);
    }

    public async Task<Location> CreateLocation(string title, string address, CancellationToken cancellationToken = default)
    {
        Location location = Location.Create(title, address);

        return await _locationRepository.Create(location, cancellationToken);
    }

    public async Task<bool> UpdateLocation(int locationId, string title, string address, CancellationToken cancellationToken = default)
    {
        Location? location = await _locationRepository.FindAsync(locationId, cancellationToken);

        if (location is null)
            throw new NotFoundException("Локация", locationId);
        
        location.UpdateLocation(title, address);
        
        return await _locationRepository.Update(location, cancellationToken);
    }

    public async Task<bool> DeleteLocation(int locationId, CancellationToken cancellationToken = default)
    {
        return await _locationRepository.Delete(locationId, cancellationToken);
    }
}