using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface IMemberRepository : IReadable<Member, (Guid invitationId, Guid studentId)>
{
    Task<List<Member>> GetAllByAuthor(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Member>> GetAllByStudent(
        Guid studentId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );
}
