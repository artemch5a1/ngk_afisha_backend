using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Entites;
using EventService.Infrastructure.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace EventService.Infrastructure.Implementations.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<LocationRepository> _logger;

    private readonly IEntityMapper<LocationEntity, Location> _entityMapper;

    public LocationRepository(
        EventServiceDbContext db,
        ILogger<LocationRepository> logger,
        IEntityMapper<LocationEntity, Location> entityMapper
    )
    {
        _db = db;
        _logger = logger;
        _entityMapper = entityMapper;
    }

    public async Task<List<Location>> GetAll(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<LocationEntity> locations = contract is null
                ? await _db.Locations.OrderBy(x => x.Title).ToListAsync(cancellationToken)
                : await _db
                    .Locations.OrderBy(x => x.Title)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .ToListAsync(cancellationToken);

            return _entityMapper.ToListDomain(locations);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локаций");
            throw ex.HandleException();
        }
    }

    public async Task<Location?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            LocationEntity? location = await _db.Locations.FirstOrDefaultAsync(
                x => x.LocationId == id,
                cancellationToken
            );

            if (location is null)
                return null;

            return _entityMapper.ToDomain(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локаций");
            throw ex.HandleException();
        }
    }

    public async Task<Location?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            LocationEntity? location = await _db.Locations.FindAsync(id, cancellationToken);

            if (location is null)
                return null;

            return _entityMapper.ToDomain(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локаций");
            throw ex.HandleException();
        }
    }

    public async Task<Location> Create(
        Location model,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            LocationEntity location = _entityMapper.ToEntity(model);

            EntityEntry<LocationEntity> locationCreated = await _db.Locations.AddAsync(
                location,
                cancellationToken
            );

            await _db.SaveChangesAsync(cancellationToken);

            return _entityMapper.ToDomain(locationCreated.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локаций");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Location model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Locations.Where(x => x.LocationId == model.LocationId)
                .ExecuteUpdateAsync(
                    x =>
                        x.SetProperty(i => i.Title, i => model.Title)
                            .SetProperty(i => i.Address, i => model.Address),
                    cancellationToken
                );

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локаций");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Locations.Where(x => x.LocationId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении локаций");
            throw ex.HandleException();
        }
    }
}
