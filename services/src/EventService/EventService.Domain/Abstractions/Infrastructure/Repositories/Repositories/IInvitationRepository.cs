using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface IInvitationRepository : IReadable<Invitation, Guid>
{
    Task<List<Invitation>> GetAllActual(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Invitation>> GetAllByAuthor(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Invitation>> GetAllByEvent(
        Guid eventId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );
}
