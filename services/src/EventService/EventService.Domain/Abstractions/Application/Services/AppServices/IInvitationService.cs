using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface IInvitationService
{
    Task<List<Invitation>> GetAllActualInvitation(
        PaginationContract? contract,
        CancellationToken cancellationToken = default
    );

    Task<List<Invitation>> GetAllInvitation(
        PaginationContract? contract,
        CancellationToken cancellationToken = default
    );

    Task<Invitation?> GetInvitationById(
        Guid invitationId,
        CancellationToken cancellationToken = default
    );

    Task<List<Invitation>> GetAllInvitationByAuthor(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Invitation>> GetAllInvitationByEvent(
        Guid eventId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );
}
