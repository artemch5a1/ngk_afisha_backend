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

public class SpecialtyRepository : ISpecialtyRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly ILogger<SpecialtyRepository> _logger;

    private readonly IEntityMapper<SpecialtyEntity, Specialty> _specialtyMapper;

    public SpecialtyRepository(
        IdentityServiceDbContext db,
        ILogger<SpecialtyRepository> logger,
        IEntityMapper<SpecialtyEntity, Specialty> specialtyMapper
    )
    {
        _db = db;
        _logger = logger;
        _specialtyMapper = specialtyMapper;
    }

    public async Task<List<Specialty>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<SpecialtyEntity> entities = await _db
                .Specialties.AsNoTracking()
                .ToListAsync(cancellationToken);

            return _specialtyMapper.ToListDomain(entities);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении специальностей");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении специальностей");
            throw ex.HandleException();
        }
    }

    public async Task<Specialty?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            SpecialtyEntity? entity = await _db
                .Specialties.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SpecialtyId == id, cancellationToken);

            if (entity is null)
                return null;

            return _specialtyMapper.ToDomain(entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении специальности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении специальности");
            throw ex.HandleException();
        }
    }

    public async Task<Specialty?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            SpecialtyEntity? entity = await _db.Specialties.FindAsync(id, cancellationToken);

            if (entity is null)
                return null;

            return _specialtyMapper.ToDomain(entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении специальности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении специальности");
            throw ex.HandleException();
        }
    }

    public async Task<Specialty> Create(
        Specialty model,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            SpecialtyEntity entity = _specialtyMapper.ToEntity(model);

            EntityEntry<SpecialtyEntity> result = await _db.Specialties.AddAsync(
                entity,
                cancellationToken
            );

            await _db.SaveChangesAsync(cancellationToken);

            return _specialtyMapper.ToDomain(result.Entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании специальности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при создании специальности");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Specialty model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Specialties.Where(x => x.SpecialtyId == model.SpecialtyId)
                .ExecuteUpdateAsync(
                    x => x.SetProperty(i => i.SpecialtyTitle, i => model.SpecialtyTitle),
                    cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении специальности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении специальности");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Specialties.Where(x => x.SpecialtyId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении специальности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при удалении специальности");
            throw ex.HandleException();
        }
    }
}
