using System.Linq.Expressions;
using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Abstractions.Specification;
using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Entites;
using EventService.Infrastructure.Extensions.Exceptions;
using EventService.Infrastructure.Implementations.SpecificationsImpl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace EventService.Infrastructure.Implementations.Repositories;

public class EventRepository : IEventRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<EventRepository> _logger;

    private readonly IEntityMapper<EventEntity, Event> _eventMapper;

    private readonly IEntityMapper<InvitationEntity, Invitation> _invitationMapper;
    private readonly EfSpecificationMapper _efSpecificationMapper;

    public EventRepository(
        EventServiceDbContext db,
        ILogger<EventRepository> logger,
        IEntityMapper<EventEntity, Event> eventMapper,
        IEntityMapper<InvitationEntity, Invitation> invitationMapper,
        EfSpecificationMapper efSpecificationMapper
    )
    {
        _db = db;
        _logger = logger;
        _eventMapper = eventMapper;
        _invitationMapper = invitationMapper;
        _efSpecificationMapper = efSpecificationMapper;
    }

    public async Task<List<Event>> GetAll(
        ISpecification<Event>? specification,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Expression<Func<EventEntity, bool>> spec = specification is null
                ? entity => true
                : _efSpecificationMapper.ResolveEfSpecification<Event, EventEntity>(specification);

            List<EventEntity> result = contract is null
                ? await _db
                    .Events.Include(x => x.Genre)
                    .Include(x => x.Type)
                    .Include(x => x.Location)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Role)
                    .Where(spec)
                    .OrderBy(x => x.DateStart)
                    .ToListAsync(cancellationToken)
                : await _db
                    .Events.Include(x => x.Genre)
                    .Include(x => x.Type)
                    .Include(x => x.Location)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Role)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .Where(spec)
                    .OrderBy(x => x.DateStart)
                    .ToListAsync(cancellationToken);

            return _eventMapper.ToListDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения событий");

            throw ex.HandleException();
        }
    }

    public async Task<List<Event>> GetAll(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<EventEntity> result = contract is null
                ? await _db
                    .Events.Include(x => x.Genre)
                    .Include(x => x.Type)
                    .Include(x => x.Location)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Role)
                    .OrderBy(x => x.DateStart)
                    .ToListAsync(cancellationToken)
                : await _db
                    .Events.Include(x => x.Genre)
                    .Include(x => x.Type)
                    .Include(x => x.Location)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Role)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .OrderBy(x => x.DateStart)
                    .ToListAsync(cancellationToken);

            return _eventMapper.ToListDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения событий");

            throw ex.HandleException();
        }
    }

    public async Task<Event?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            EventEntity? result = await _db
                .Events.Include(x => x.Genre)
                .Include(x => x.Type)
                .Include(x => x.Location)
                .Include(x => x.Invitations)
                    .ThenInclude(x => x.Members)
                .Include(x => x.Invitations)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.EventId == id, cancellationToken);

            if (result is null)
                return null;

            return _eventMapper.ToDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения события по айди");

            throw ex.HandleException();
        }
    }

    public async Task<Event?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            EventEntity? result = await _db.Events.FindAsync(id, cancellationToken);

            if (result is null)
                return null;

            return _eventMapper.ToDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения события по айди");

            throw ex.HandleException();
        }
    }

    public async Task<Event> Create(Event model, CancellationToken cancellationToken = default)
    {
        try
        {
            EventEntity eventEntity = _eventMapper.ToEntity(model);

            EntityEntry<EventEntity> createdEvent = await _db.Events.AddAsync(
                eventEntity,
                cancellationToken
            );

            await _db.SaveChangesAsync(cancellationToken);

            return _eventMapper.ToDomain(createdEvent.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения события по айди");

            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Event model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Events.Where(x => x.EventId == model.EventId)
                .ExecuteUpdateAsync(
                    x =>
                        x.SetProperty(i => i.Title, i => model.Title)
                            .SetProperty(i => i.ShortDescription, i => model.ShortDescription)
                            .SetProperty(i => i.Description, i => model.Description)
                            .SetProperty(i => i.DateStart, i => model.DateStart)
                            .SetProperty(i => i.LocationId, i => model.LocationId)
                            .SetProperty(i => i.GenreId, i => model.GenreId)
                            .SetProperty(i => i.TypeId, i => model.TypeId)
                            .SetProperty(i => i.MinAge, i => model.MinAge)
                            .SetProperty(i => i.PreviewUrl, i => model.PreviewUrl),
                    cancellationToken
                );

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения события по айди");

            throw ex.HandleException();
        }
    }

    public async Task<bool> UpdateEventAggregate(
        Event updatedDomainEvent,
        CancellationToken ct = default
    )
    {
        try
        {
            var existingEvent = await _db
                .Events.Include(e => e.Invitations)
                    .ThenInclude(i => i.Members)
                .FirstOrDefaultAsync(e => e.EventId == updatedDomainEvent.EventId, ct);

            if (existingEvent == null)
                return false;

            await UpdateInvitations(existingEvent, updatedDomainEvent);

            _logger.LogInformation(_db.ChangeTracker.DebugView.LongView);

            int result = await _db.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка обновления агрегата");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db
                .Events.Where(x => x.EventId == id)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения события по айди");

            throw ex.HandleException();
        }
    }

    public async Task<List<Event>> GetAllEventByAuthorId(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<EventEntity> result = contract is null
                ? await _db
                    .Events.Include(x => x.Genre)
                    .Include(x => x.Type)
                    .Include(x => x.Location)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Role)
                    .Where(x => x.Author == authorId)
                    .OrderBy(x => x.DateStart)
                    .ToListAsync(cancellationToken)
                : await _db
                    .Events.Include(x => x.Genre)
                    .Include(x => x.Type)
                    .Include(x => x.Location)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Invitations)
                        .ThenInclude(x => x.Role)
                    .Where(x => x.Author == authorId)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .OrderBy(x => x.DateStart)
                    .ToListAsync(cancellationToken);

            return _eventMapper.ToListDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения событий");

            throw ex.HandleException();
        }
    }

    public async Task<bool> EventsIsExist(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _db.Events.AnyAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка получения событий");

            throw ex.HandleException();
        }
    }

    private async Task UpdateInvitations(EventEntity existingEvent, Event updatedDomainEvent)
    {
        existingEvent.Invitations ??= new List<InvitationEntity>();

        var updatedInvitations = updatedDomainEvent
            .Invitations.Select(_invitationMapper.ToEntity)
            .ToList();

        RemoveDeletedInvitations(existingEvent, updatedInvitations);

        foreach (InvitationEntity updatedInvitation in updatedInvitations)
        {
            InvitationEntity? existingInvitation = existingEvent.Invitations.FirstOrDefault(i =>
                i.InvitationId == updatedInvitation.InvitationId
            );

            if (existingInvitation == null)
            {
                existingEvent.Invitations.Add(updatedInvitation);
                await _db.Invitations.AddAsync(updatedInvitation);
            }
            else
            {
                UpdateInvitationFields(existingInvitation, updatedInvitation);

                await UpdateMembers(existingInvitation, updatedInvitation);
            }
        }
    }

    private void RemoveDeletedInvitations(
        EventEntity existingEvent,
        List<InvitationEntity> updatedInvitations
    )
    {
        foreach (InvitationEntity existingInvitation in existingEvent.Invitations.ToList())
        {
            if (updatedInvitations.All(i => i.InvitationId != existingInvitation.InvitationId))
            {
                _db.Invitations.Remove(existingInvitation);
            }
        }
    }

    private static void UpdateInvitationFields(InvitationEntity existing, InvitationEntity updated)
    {
        existing.RoleId = updated.RoleId;
        existing.ShortDescription = updated.ShortDescription;
        existing.Description = updated.Description;
        existing.RequiredMember = updated.RequiredMember;
        existing.AcceptedMember = updated.AcceptedMember;
        existing.DeadLine = updated.DeadLine;
        existing.Status = updated.Status;
    }

    private async Task UpdateMembers(
        InvitationEntity existingInvitation,
        InvitationEntity updatedInvitation
    )
    {
        List<MemberEntity> updatedMembers = updatedInvitation.Members.ToList();

        foreach (MemberEntity existingMember in existingInvitation.Members.ToList())
        {
            if (
                !updatedMembers.Any(m =>
                    m.InvitationId == existingMember.InvitationId
                    && m.StudentId == existingMember.StudentId
                )
            )
            {
                _db.Members.Remove(existingMember);
            }
        }

        foreach (MemberEntity updatedMember in updatedMembers)
        {
            MemberEntity? existingMember = existingInvitation.Members.FirstOrDefault(m =>
                m.InvitationId == updatedMember.InvitationId
                && m.StudentId == updatedMember.StudentId
            );

            if (existingMember == null)
            {
                existingInvitation.Members.Add(updatedMember);
                await _db.Members.AddAsync(updatedMember);
            }
            else
            {
                existingMember.Status = updatedMember.Status;
            }
        }
    }
}
