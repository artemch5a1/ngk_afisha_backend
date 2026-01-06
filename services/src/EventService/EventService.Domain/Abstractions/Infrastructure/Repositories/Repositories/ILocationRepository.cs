using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface ILocationRepository : 
    IReadable<Location, int>, 
    IWritable<Location, int>, 
    IUpdatable<Location>, 
    IDeletable<int>;