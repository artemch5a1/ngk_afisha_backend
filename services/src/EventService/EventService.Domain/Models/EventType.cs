using EventService.Domain.CustomExceptions;

namespace EventService.Domain.Models;

/// <summary>
/// Представляет тип события с названием.
/// </summary>
public class EventType
{
    private const int MinTitleLength = 2;
    private const int MaxTitleLength = 45;
    
    public int TypeId { get; private set; }

    public string Title { get; private set; }

    private EventType(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Создаёт новый тип события с указанным названием.
    /// </summary>
    /// <param name="title">Название типа события.</param>
    /// <returns>Новый экземпляр <see cref="EventType"/>.</returns>
    /// <exception cref="DomainException">
    /// Выбрасывается, если название не удовлетворяет ограничениям по длине.
    /// </exception>
    public static EventType Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            throw new DomainException(MaxMinMessage(
                "Название типа события",  
                MinTitleLength, 
                MaxTitleLength));
        }
        
        return new EventType(title);
    }
    
    /// <summary>
    /// Восстанавливает существующий тип события (например, из базы данных).
    /// </summary>
    /// <param name="typeId">Идентификатор типа события.</param>
    /// <param name="title">Название типа события.</param>
    /// <returns>Восстановленный экземпляр <see cref="EventType"/>.</returns>
    internal static EventType Restore(int typeId, string title)
    {
        return new EventType(title)
        {
            TypeId = typeId,
        };
    }

    private static string MaxMinMessage(object someObject, int min, int max)
        => $"{someObject} не должно быть меньше {min} или больше {max} символов";
    
    /// <summary>
    /// Обновляет название типа события.
    /// </summary>
    /// <param name="title">Новое название типа события.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если новое название не удовлетворяет ограничениям по длине.
    /// </exception>
    public void UpdateEventType(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            throw new DomainException(MaxMinMessage(
                "Название типа события",  
                MinTitleLength, 
                MaxTitleLength));
        }
        
        Title = title;
    }
}