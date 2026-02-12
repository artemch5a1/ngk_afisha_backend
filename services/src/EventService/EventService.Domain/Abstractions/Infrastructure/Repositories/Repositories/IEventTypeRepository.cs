using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface IEventTypeRepository
    : IReadable<EventType, int>,
        IWritable<EventType, int>,
        IUpdatable<EventType>,
        IDeletable<int>;
