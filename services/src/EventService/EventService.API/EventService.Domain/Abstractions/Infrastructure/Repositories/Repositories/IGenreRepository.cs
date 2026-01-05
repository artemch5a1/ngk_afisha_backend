using EventService.Domain.Abstractions.Infrastructure.Repositories.Base;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;

public interface IGenreRepository: 
    IReadable<Genre, int>, 
    IWritable<Genre, int>, 
    IUpdatable<Genre>, 
    IDeletable<int>;