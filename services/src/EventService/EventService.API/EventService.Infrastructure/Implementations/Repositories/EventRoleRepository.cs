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

public class EventRoleRepository : IEventRoleRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<EventRoleRepository> _logger;

    private readonly IEntityMapper<EventRoleEntity, EventRole> _roleMapper;

    public EventRoleRepository(
        EventServiceDbContext db, 
        ILogger<EventRoleRepository> logger, 
        IEntityMapper<EventRoleEntity, EventRole> roleMapper)
    {
        _db = db;
        _logger = logger;
        _roleMapper = roleMapper;
    }

    public async Task<List<EventRole>> GetAll(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        try
        {
            List<EventRoleEntity> entities = contract is null ?
                await _db.EventRoles
                    .AsNoTracking()
                    .ToListAsync(cancellationToken) :
                await _db.EventRoles
                    .AsNoTracking().
                    Skip(contract.Skip)
                    .Take(contract.Take)
                    .ToListAsync(cancellationToken);

            return _roleMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении всех ролей");
            throw ex.HandleException();
        }
    }

    public async Task<EventRole?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            EventRoleEntity? entity = 
                await _db.EventRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EventRoleId == id ,cancellationToken);

            if (entity is null)
                return null;

            return _roleMapper.ToDomain(entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении роли по id");
            throw ex.HandleException();
        }
    }

    public async Task<EventRole?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            EventRoleEntity? entity = 
                await _db.EventRoles
                    .FindAsync(id ,cancellationToken);

            if (entity is null)
                return null;

            return _roleMapper.ToDomain(entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении роли по id");
            throw ex.HandleException();
        }
    }

    public async Task<EventRole> Create(EventRole model, CancellationToken cancellationToken = default)
    {
        try
        {
            EventRoleEntity entity = _roleMapper.ToEntity(model);

            EntityEntry<EventRoleEntity> result = await _db.EventRoles.AddAsync(entity, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            return _roleMapper.ToDomain(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании роли");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(EventRole model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.EventRoles
                .Where(x => x.EventRoleId == model.EventRoleId)
                .ExecuteUpdateAsync(
                    x => x
                        .SetProperty(i => i.Title, i => model.Title)
                        .SetProperty(i => i.Description, i => model.Description),
                    cancellationToken
                );

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении роли");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.EventRoles
                .Where(x => x.EventRoleId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении роли");
            throw ex.HandleException();
        }
    }
}