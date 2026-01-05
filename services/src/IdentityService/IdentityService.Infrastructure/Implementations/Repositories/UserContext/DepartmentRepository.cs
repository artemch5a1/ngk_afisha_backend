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

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly IEntityMapper<DepartmentEntity, Department> _departmentEntity;

    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(
        IdentityServiceDbContext db, 
        IEntityMapper<DepartmentEntity, Department> departmentEntity, 
        ILogger<DepartmentRepository> logger)
    {
        _db = db;
        _departmentEntity = departmentEntity;
        _logger = logger;
    }

    public async Task<List<Department>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<DepartmentEntity> result = await _db.Departments.ToListAsync(cancellationToken);

            return _departmentEntity.ToListDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении отделов");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении отделов");
            throw ex.HandleException();
        }
    }

    public async Task<Department?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            DepartmentEntity? result = await _db.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id, cancellationToken);

            if (result is null)
                return null;
            
            return _departmentEntity.ToDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении отдела");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении отдела");
            throw ex.HandleException();
        }
    }

    public async Task<Department?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            DepartmentEntity? result = await _db.Departments.FindAsync(id, cancellationToken);

            if (result is null)
                return null;
            
            return _departmentEntity.ToDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении отдела");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении отдела");
            throw ex.HandleException();
        }
    }

    public async Task<Department> Create(Department model, CancellationToken cancellationToken = default)
    {
        try
        {
            DepartmentEntity entity = _departmentEntity.ToEntity(model);

            EntityEntry<DepartmentEntity> createdEntity = await _db.Departments.AddAsync(entity, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
            
            return _departmentEntity.ToDomain(createdEntity.Entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании отдела");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при создании отдела");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Department model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.Departments
                .Where(x => x.DepartmentId == model.DepartmentId)
                .ExecuteUpdateAsync(
                x => x
                    .SetProperty(i => i.Title, i => model.Title),
                cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении отдела");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при обновлении отдела");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.Departments
                .Where(x => x.DepartmentId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении отдела");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при удалении отдела");
            throw ex.HandleException();
        }
    }
}