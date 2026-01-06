using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    public async Task<List<Genre>> GetAllGenre(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        return await _genreRepository.GetAll(contract, cancellationToken);
    }

    public async Task<Genre?> GetGenreById(int genreId, CancellationToken cancellationToken = default)
    {
        return await _genreRepository.GetById(genreId, cancellationToken);
    }

    public async Task<Genre> CreateGenre(string title, CancellationToken cancellationToken = default)
    {
        Genre genre = Genre.Create(title);
        
        return await _genreRepository.Create(genre, cancellationToken);
    }

    public async Task<bool> UpdateGenre(int genreId, string title, CancellationToken cancellationToken = default)
    {
        Genre? genre = await _genreRepository.FindAsync(genreId, cancellationToken);

        if (genre is null)
            throw new NotFoundException("Жанр", genreId);
        
        genre.UpdateGenre(title);

        return await _genreRepository.Update(genre, cancellationToken);
    }

    public async Task<bool> DeleteGenre(int genreId, CancellationToken cancellationToken = default)
    {
        return await _genreRepository.Delete(genreId, cancellationToken);
    }
}