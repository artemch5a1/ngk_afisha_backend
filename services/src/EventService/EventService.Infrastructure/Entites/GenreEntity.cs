using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class GenreEntity : IEntity<GenreEntity, Genre>
{
    [Column("genre_id")]
    public int GenreId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    internal GenreEntity() { }

    private GenreEntity(Genre genre)
    {
        Title = genre.Title;
    }

    public Genre ToDomain()
    {
        return Genre.Restore(GenreId, Title);
    }

    public static GenreEntity ToEntity(Genre domain)
    {
        return new GenreEntity(domain);
    }
}
