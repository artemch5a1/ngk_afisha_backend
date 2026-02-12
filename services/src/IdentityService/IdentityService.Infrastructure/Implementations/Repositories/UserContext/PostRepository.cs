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

public class PostRepository : IPostRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly ILogger<PostRepository> _logger;

    private readonly IEntityMapper<PostEntity, Post> _postMapper;

    public PostRepository(
        IdentityServiceDbContext db,
        ILogger<PostRepository> logger,
        IEntityMapper<PostEntity, Post> postMapper
    )
    {
        _db = db;
        _logger = logger;
        _postMapper = postMapper;
    }

    public async Task<List<Post>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<PostEntity> result = await _db
                .Posts.Include(x => x.Department)
                .ToListAsync(cancellationToken);

            return _postMapper.ToListDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении должностей");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении должностей");
            throw ex.HandleException();
        }
    }

    public async Task<Post?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            PostEntity? result = await _db
                .Posts.Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.PostId == id, cancellationToken);

            if (result is null)
                return null;

            return _postMapper.ToDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении должности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении должности");
            throw ex.HandleException();
        }
    }

    public async Task<Post?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            PostEntity? result = await _db.Posts.FindAsync(id, cancellationToken);

            if (result is null)
                return null;

            return _postMapper.ToDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении должности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении должности");
            throw ex.HandleException();
        }
    }

    public async Task<Post> Create(Post model, CancellationToken cancellationToken = default)
    {
        try
        {
            PostEntity postEntity = _postMapper.ToEntity(model);

            EntityEntry<PostEntity> createdEntity = await _db.Posts.AddAsync(
                postEntity,
                cancellationToken
            );

            await _db.SaveChangesAsync(cancellationToken);

            return _postMapper.ToDomain(createdEntity.Entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании должности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при создании должности");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Post model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Posts.Where(x => x.PostId == model.PostId)
                .ExecuteUpdateAsync(
                    x =>
                        x.SetProperty(i => i.Title, i => model.Title)
                            .SetProperty(i => i.DepartmentId, i => model.DepartmentId),
                    cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении должности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при обновлении должности");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Posts.Where(x => x.PostId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении должности");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при обновлении должности");
            throw ex.HandleException();
        }
    }

    public async Task<List<Post>> GetAllPostByDepartmentId(
        int departmentId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<PostEntity> result = await _db
                .Posts.Where(x => x.DepartmentId == departmentId)
                .Include(x => x.Department)
                .ToListAsync(cancellationToken);

            return _postMapper.ToListDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении должностей в отделе");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при получении должностей в отделе");
            throw ex.HandleException();
        }
    }
}
