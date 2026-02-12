using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Abstractions.Specification;
using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface IEventRepository
    : IReadable<Event, Guid>,
        IWritable<Event, Guid>,
        IUpdatable<Event>,
        IDeletable<Guid>
{
    Task<List<Event>> GetAll(
        ISpecification<Event>? specification,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Event>> GetAllEventByAuthorId(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<bool> UpdateEventAggregate(Event updatedDomainEvent, CancellationToken ct = default);

    Task<bool> EventsIsExist(CancellationToken cancellationToken = default);
}
