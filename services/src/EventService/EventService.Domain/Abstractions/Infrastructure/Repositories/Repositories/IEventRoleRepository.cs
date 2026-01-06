using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface IEventRoleRepository : 
    IReadable<EventRole, int>, 
    IWritable<EventRole, int>, 
    IUpdatable<EventRole>, 
    IDeletable<int>;