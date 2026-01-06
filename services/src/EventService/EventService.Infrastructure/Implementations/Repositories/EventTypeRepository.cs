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

public class EventTypeRepository : IEventTypeRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<EventTypeRepository> _logger;

    private readonly IEntityMapper<EventTypeEntity, EventType> _entityMapper;
    
    public EventTypeRepository(
        EventServiceDbContext db, 
        ILogger<EventTypeRepository> logger, 
        IEntityMapper<EventTypeEntity, EventType> entityMapper)
    {
        _db = db;
        _logger = logger;
        _entityMapper = entityMapper;
    }

    public async Task<List<EventType>> GetAll(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        try
        {
            List<EventTypeEntity> result = contract is null ? 
                await _db.EventTypes
                    .OrderBy(x => x.Title)
                    .ToListAsync(cancellationToken) : 
                await _db.EventTypes
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .OrderBy(x => x.Title)
                    .ToListAsync(cancellationToken);

            return _entityMapper.ToListDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения типов событий событий");
            
            throw ex.HandleException();
        }
    }

    public async Task<EventType?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            EventTypeEntity? result = await _db.EventTypes
                .FirstOrDefaultAsync(x => x.TypeId == id, cancellationToken);

            if (result is null)
                return null;
            
            return _entityMapper.ToDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения типа события по айди");
            
            throw ex.HandleException();
        }
    }

    public async Task<EventType?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            EventTypeEntity? result = await _db.EventTypes
                .FindAsync(id, cancellationToken);

            if (result is null)
                return null;
            
            return _entityMapper.ToDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения типа события по ключу");
            
            throw ex.HandleException();
        }
    }

    public async Task<EventType> Create(EventType model, CancellationToken cancellationToken = default)
    {
        try
        {
            EventTypeEntity eventTypeEntity = _entityMapper.ToEntity(model);

            EntityEntry<EventTypeEntity> createdResult =
                await _db.EventTypes.AddAsync(eventTypeEntity, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
            
            return _entityMapper.ToDomain(createdResult.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка создания типа события");
            
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(EventType model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.EventTypes
                .Where(x => x.TypeId == model.TypeId)
                .ExecuteUpdateAsync(
                    x => x
                        .SetProperty(i => i.Title, i => model.Title),
                    cancellationToken
                );

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка обновления типа события");
            
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.EventTypes
                .Where(x => x.TypeId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка удаления типа события");
            
            throw ex.HandleException();
        }
    }
}