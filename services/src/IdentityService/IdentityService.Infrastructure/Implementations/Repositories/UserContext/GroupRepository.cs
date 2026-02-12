using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Implementations.Repositories.UserContext;

public class GroupRepository : IGroupRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly ILogger<GroupRepository> _logger;

    private readonly IEntityMapper<GroupEntity, Group> _groupMapper;

    public GroupRepository(
        IdentityServiceDbContext db,
        ILogger<GroupRepository> logger,
        IEntityMapper<GroupEntity, Group> groupMapper
    )
    {
        _db = db;
        _logger = logger;
        _groupMapper = groupMapper;
    }

    public async Task<List<Group>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<GroupEntity> entities = await _db
                .Groups.AsNoTracking()
                .Include(x => x.Specialty)
                .ToListAsync(cancellationToken);

            return _groupMapper.ToListDomain(entities);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении групп");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении групп");
            throw ex.HandleException();
        }
    }

    public async Task<List<Group>> GetAllBySpecialtyId(
        int specialtyId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<GroupEntity> entities = await _db
                .Groups.Where(x => x.SpecialtyId == specialtyId)
                .AsNoTracking()
                .Include(x => x.Specialty)
                .ToListAsync(cancellationToken);

            return _groupMapper.ToListDomain(entities);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении групп");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении групп");
            throw ex.HandleException();
        }
    }

    public async Task<Group?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            GroupEntity? entity = await _db
                .Groups.AsNoTracking()
                .Include(x => x.Specialty)
                .FirstOrDefaultAsync(x => x.GroupId == id, cancellationToken);

            if (entity is null)
                return null;

            return _groupMapper.ToDomain(entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении группы");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении группы");
            throw ex.HandleException();
        }
    }

    public async Task<Group?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            GroupEntity? entity = await _db.Groups.FindAsync(id, cancellationToken);

            if (entity is null)
                return null;

            return _groupMapper.ToDomain(entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении группы");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении группы");
            throw ex.HandleException();
        }
    }

    public async Task<Group> Create(Group model, CancellationToken cancellationToken = default)
    {
        try
        {
            GroupEntity entity = _groupMapper.ToEntity(model);

            EntityEntry<GroupEntity> result = await _db.Groups.AddAsync(entity, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            GroupEntity createdEntity = await _db
                .Groups.AsNoTracking()
                .Include(x => x.Specialty)
                .FirstAsync(x => x.GroupId == result.Entity.GroupId, cancellationToken);

            return _groupMapper.ToDomain(createdEntity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании группы");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при создании группы");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Group model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Groups.Where(x => x.GroupId == model.GroupId)
                .ExecuteUpdateAsync(
                    x =>
                        x.SetProperty(i => i.Course, i => model.Course)
                            .SetProperty(i => i.NumberGroup, i => model.NumberGroup)
                            .SetProperty(i => i.SpecialtyId, i => model.SpecialtyId),
                    cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении группы");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении группы");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Groups.Where(x => x.GroupId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении группы");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении группы");
            throw ex.HandleException();
        }
    }
}
