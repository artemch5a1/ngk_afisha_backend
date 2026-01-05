namespace EventService.API.Contracts.Genres;

public class UpdateGenreDto
{
    public int GenreId { get; set; }

    public string Title { get; set; } = null!;
}