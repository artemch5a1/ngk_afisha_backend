using EventService.API.Contracts.Genres;
using EventService.Application.UseCases.GenreCases.CreateGenre;
using EventService.Application.UseCases.GenreCases.UpdateGenre;
using EventService.Domain.Models;

namespace EventService.API.Extensions.Mappings;

public static class GenreMapper
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto() { GenreId = genre.GenreId, Title = genre.Title };
    }

    public static List<GenreDto> ToListDto(this List<Genre> genres) =>
        genres.Select(x => x.ToDto()).ToList();

    public static UpdateGenreCommand ToCommand(this UpdateGenreDto dto)
    {
        return new UpdateGenreCommand(dto.GenreId, dto.Title);
    }

    public static CreateGenreCommand ToCommand(this CreateGenreDto dto)
    {
        return new CreateGenreCommand(dto.Title);
    }
}
