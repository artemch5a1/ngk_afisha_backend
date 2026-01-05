using EventService.Domain.CustomExceptions;

namespace EventService.Domain.Models;

/// <summary>
/// Представляет жанр события с названием.
/// </summary>
public class Genre
{
    private const int MinTitleLength = 2;
    private const int MaxTitleLength = 45;
    
    public int GenreId { get; private set; }

    public string Title { get; private set; }

    private Genre(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Создаёт новый жанр с указанным названием.
    /// </summary>
    /// <param name="title">Название жанра.</param>
    /// <returns>Новый экземпляр <see cref="Genre"/>.</returns>
    /// <exception cref="DomainException">
    /// Выбрасывается, если название не удовлетворяет ограничениям по длине.
    /// </exception>
    public static Genre Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            throw new DomainException(MaxMinMessage(
                "Название жанра",  
                MinTitleLength, 
                MaxTitleLength));
        }
        
        return new Genre(title);
    }

    /// <summary>
    /// Восстанавливает существующий жанр (например, из базы данных).
    /// </summary>
    /// <param name="genreId">Идентификатор жанра.</param>
    /// <param name="title">Название жанра.</param>
    /// <returns>Восстановленный экземпляр <see cref="Genre"/>.</returns>
    internal static Genre Restore(int genreId, string title)
    {
        return new Genre(title)
        {
            GenreId = genreId,
        };
    }
    
    private static string MaxMinMessage(object someObject, int min, int max)
        => $"{someObject} не должно быть меньше {min} или больше {max} символов";

    /// <summary>
    /// Обновляет название жанра.
    /// </summary>
    /// <param name="title">Новое название жанра.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если новое название не удовлетворяет ограничениям по длине.
    /// </exception>
    public void UpdateGenre(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            throw new DomainException(MaxMinMessage(
                "Название жанра",  
                MinTitleLength, 
                MaxTitleLength));
        }
        
        Title = title;
    }
}