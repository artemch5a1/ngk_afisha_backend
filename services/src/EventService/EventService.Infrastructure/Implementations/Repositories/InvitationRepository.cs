using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Entites;
using EventService.Infrastructure.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventService.Infrastructure.Implementations.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<InvitationRepository> _logger;

    private readonly IEntityMapper<InvitationEntity, Invitation> _invitationMapper;
    
    public InvitationRepository(
        EventServiceDbContext db, ILogger<InvitationRepository> logger, 
        IEntityMapper<InvitationEntity, Invitation> invitationMapper)
    {
        _db = db;
        _logger = logger;
        _invitationMapper = invitationMapper;
    }

    public async Task<List<Invitation>> GetAllActual(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        try
        {
            List<InvitationEntity> entities = contract is null ?
                await _db.Invitations
                    .AsNoTracking()
                    .Where(x => x.Status == (int)InvitationStatus.Active && x.DeadLine > DateTime.UtcNow)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken)
                :
                await _db.Invitations
                    .AsNoTracking()
                    .Where(x => x.Status == (int)InvitationStatus.Active && x.DeadLine > DateTime.UtcNow)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken);

            return _invitationMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении приглашений");

            throw ex.HandleException();
        }
    }
    
    public async Task<List<Invitation>> GetAll(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        try
        {
            List<InvitationEntity> entities = contract is null ?
                await _db.Invitations
                    .AsNoTracking()
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken)
                :
                await _db.Invitations
                    .AsNoTracking()
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken);

            return _invitationMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении приглашений");

            throw ex.HandleException();
        }
    }

    public async Task<Invitation?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        InvitationEntity? invitation = 
            await _db.Invitations
                .AsNoTracking()
                .Include(x => x.Event)
                .ThenInclude(x => x.Genre)
                .Include(x => x.Event)
                .ThenInclude(x => x.Location)
                .Include(x => x.Event)
                .ThenInclude(x => x.Type)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.InvitationId == id, cancellationToken);

        if (invitation is null)
            return null;
        
        return _invitationMapper.ToDomain(invitation);
    }

    public async Task<Invitation?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        InvitationEntity? invitation = 
            await _db.Invitations
                .FindAsync(id, cancellationToken);

        if (invitation is null)
            return null;
        
        return _invitationMapper.ToDomain(invitation);
    }
    
    public async Task<List<Invitation>> GetAllByAuthor(Guid authorId, PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        try
        {
            List<InvitationEntity> entities = contract is null ?
                await _db.Invitations
                    .AsNoTracking()
                    .Where(x => x.Event.Author == authorId)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken)
                :
                await _db.Invitations
                    .AsNoTracking()
                    .Where(x => x.Event.Author == authorId)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken);

            return _invitationMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении приглашений");

            throw ex.HandleException();
        }
    }
    
    public async Task<List<Invitation>> GetAllByEvent(Guid eventId, PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        try
        {
            List<InvitationEntity> entities = contract is null ?
                await _db.Invitations
                    .AsNoTracking()
                    .Where(x => x.EventId == eventId)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken)
                :
                await _db.Invitations
                    .AsNoTracking()
                    .Where(x => x.EventId == eventId)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Role)
                    .OrderBy(x => x.DeadLine)
                    .ToListAsync(cancellationToken);

            return _invitationMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении приглашений");

            throw ex.HandleException();
        }
    }
}