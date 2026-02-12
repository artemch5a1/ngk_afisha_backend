using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Infrastructure.Data.Database;
using EventService.Infrastructure.Entites;
using EventService.Infrastructure.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventService.Infrastructure.Implementations.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly EventServiceDbContext _db;

    private readonly ILogger<MemberRepository> _logger;

    private readonly IEntityMapper<MemberEntity, Member> _entityMapper;

    public MemberRepository(
        EventServiceDbContext db,
        ILogger<MemberRepository> logger,
        IEntityMapper<MemberEntity, Member> entityMapper
    )
    {
        _db = db;
        _logger = logger;
        _entityMapper = entityMapper;
    }

    public async Task<List<Member>> GetAll(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<MemberEntity> entities = contract is null
                ? await _db
                    .Members.AsNoTracking()
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Genre)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Location)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Type)
                    .ToListAsync(cancellationToken)
                : await _db
                    .Members.AsNoTracking()
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Genre)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Location)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Type)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .ToListAsync(cancellationToken);

            return _entityMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участников");

            throw ex.HandleException();
        }
    }

    public async Task<Member?> GetById(
        (Guid invitationId, Guid studentId) id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            MemberEntity? memberEntity = await _db
                .Members.AsNoTracking()
                .Include(x => x.Invitation)
                    .ThenInclude(x => x.Role)
                .Include(x => x.Invitation)
                    .ThenInclude(x => x.Event)
                .Include(x => x.Invitation)
                    .ThenInclude(x => x.Event)
                .Include(x => x.Invitation)
                    .ThenInclude(x => x.Event)
                        .ThenInclude(x => x.Genre)
                .Include(x => x.Invitation)
                    .ThenInclude(x => x.Event)
                        .ThenInclude(x => x.Location)
                .Include(x => x.Invitation)
                    .ThenInclude(x => x.Event)
                        .ThenInclude(x => x.Type)
                .FirstOrDefaultAsync(
                    x => x.InvitationId == id.invitationId && x.StudentId == id.studentId,
                    cancellationToken
                );

            if (memberEntity is null)
                return null;

            return _entityMapper.ToDomain(memberEntity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участника");

            throw ex.HandleException();
        }
    }

    public async Task<Member?> FindAsync(
        (Guid invitationId, Guid studentId) id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            MemberEntity? memberEntity = await _db.Members.FindAsync(
                id.invitationId,
                id.studentId,
                cancellationToken
            );

            if (memberEntity is null)
                return null;

            return _entityMapper.ToDomain(memberEntity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участника");

            throw ex.HandleException();
        }
    }

    public async Task<List<Member>> GetAllByAuthor(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<MemberEntity> entities = contract is null
                ? await _db
                    .Members.AsNoTracking()
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Genre)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Location)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Type)
                    .Where(x => x.Invitation.Event.Author == authorId)
                    .ToListAsync(cancellationToken)
                : await _db
                    .Members.AsNoTracking()
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Genre)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Location)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Type)
                    .Where(x => x.Invitation.Event.Author == authorId)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .ToListAsync(cancellationToken);

            return _entityMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участников");

            throw ex.HandleException();
        }
    }

    public async Task<List<Member>> GetAllByStudent(
        Guid studentId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<MemberEntity> entities = contract is null
                ? await _db
                    .Members.AsNoTracking()
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Genre)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Location)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Type)
                    .Where(x => x.StudentId == studentId)
                    .ToListAsync(cancellationToken)
                : await _db
                    .Members.AsNoTracking()
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Genre)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Location)
                    .Include(x => x.Invitation)
                        .ThenInclude(x => x.Event)
                            .ThenInclude(x => x.Type)
                    .Where(x => x.StudentId == studentId)
                    .Skip(contract.Skip)
                    .Take(contract.Take)
                    .ToListAsync(cancellationToken);

            return _entityMapper.ToListDomain(entities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участников");

            throw ex.HandleException();
        }
    }
}
