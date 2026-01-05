using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface IGenreService
{
    Task<List<Genre>> GetAllGenre(PaginationContract? contract = null, CancellationToken cancellationToken = default);

    Task<Genre?> GetGenreById(int genreId, CancellationToken cancellationToken = default);

    Task<Genre> CreateGenre(string title,CancellationToken cancellationToken = default);

    Task<bool> UpdateGenre(int genreId, string title, CancellationToken cancellationToken = default);

    Task<bool> DeleteGenre(int genreId, CancellationToken cancellationToken = default);
}