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

public class GenreRepository : IGenreRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<GenreRepository> _logger;

    private readonly IEntityMapper<GenreEntity, Genre> _entityMapper;

    public GenreRepository(
        EventServiceDbContext db,
        ILogger<GenreRepository> logger,
        IEntityMapper<GenreEntity, Genre> entityMapper
    )
    {
        _db = db;
        _logger = logger;
        _entityMapper = entityMapper;
    }

    public async Task<List<Genre>> GetAll(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<GenreEntity> result = contract is null
                ? await _db.Genres.OrderBy(x => x.Title).ToListAsync(cancellationToken)
                : await _db
                    .Genres.Skip(contract.Skip)
                    .Take(contract.Take)
                    .OrderBy(x => x.Title)
                    .ToListAsync(cancellationToken);

            return _entityMapper.ToListDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения жанров");

            throw ex.HandleException();
        }
    }

    public async Task<Genre?> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            GenreEntity? result = await _db.Genres.FirstOrDefaultAsync(
                x => x.GenreId == id,
                cancellationToken
            );

            if (result is null)
                return null;

            return _entityMapper.ToDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения жанра по id");

            throw ex.HandleException();
        }
    }

    public async Task<Genre?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            GenreEntity? result = await _db.Genres.FindAsync(id, cancellationToken);

            if (result is null)
                return null;

            return _entityMapper.ToDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения жанра по ключу");

            throw ex.HandleException();
        }
    }

    public async Task<Genre> Create(Genre model, CancellationToken cancellationToken = default)
    {
        try
        {
            GenreEntity eventTypeEntity = _entityMapper.ToEntity(model);

            EntityEntry<GenreEntity> createdResult = await _db.Genres.AddAsync(
                eventTypeEntity,
                cancellationToken
            );

            await _db.SaveChangesAsync(cancellationToken);

            return _entityMapper.ToDomain(createdResult.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка создания жанра");

            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Genre model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Genres.Where(x => x.GenreId == model.GenreId)
                .ExecuteUpdateAsync(
                    x => x.SetProperty(i => i.Title, i => model.Title),
                    cancellationToken
                );

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка обновления жанра");

            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Genres.Where(x => x.GenreId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка удаления жанра");

            throw ex.HandleException();
        }
    }
}
