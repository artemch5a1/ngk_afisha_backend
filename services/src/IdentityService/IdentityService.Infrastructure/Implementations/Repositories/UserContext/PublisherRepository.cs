using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Implementations.Repositories.UserContext;

public class PublisherRepository : IPublisherRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly ILogger<PublisherRepository> _logger;

    private readonly IEntityMapper<PublisherEntity, Publisher> _publisherMapper;

    public PublisherRepository(
        IdentityServiceDbContext db, 
        ILogger<PublisherRepository> logger, 
        IEntityMapper<PublisherEntity, Publisher> publisherMapper)
    {
        _db = db;
        _logger = logger;
        _publisherMapper = publisherMapper;
    }

    public async Task<List<Publisher>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<PublisherEntity> publishers = await _db.Publishers
                .Include(x => x.User)
                .Include(x => x.Post)
                .ThenInclude(x => x.Department)
                .ToListAsync(cancellationToken);

            return _publisherMapper.ToListDomain(publishers);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении публикаторов");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении публикаторов");
            throw ex.HandleException();
        }
    }

    public async Task<Publisher?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            PublisherEntity? publisher = await _db.Publishers
                .Include(x => x.User)
                .Include(x => x.Post)
                .ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.PublisherId == id, cancellationToken);

            if (publisher is null)
                return null;

            return _publisherMapper.ToDomain(publisher);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении публикатора");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении публикатора");
            throw ex.HandleException();
        }
    }

    public async Task<Publisher?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            PublisherEntity? publisher = await _db.Publishers
                .FindAsync(id, cancellationToken);

            if (publisher is null)
                return null;

            return _publisherMapper.ToDomain(publisher);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при поиске публикатора");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при поиске публикатора");
            throw ex.HandleException();
        }
    }
}