using EventService.Domain.CustomExceptions;

namespace EventService.Domain.Models;

/// <summary>
/// Представляет локацию проведения мероприятия.
/// </summary>
public class Location
{
    private const int MinTitleLength = 4;
    private const int MaxTitleLength = 45;
    
    public int LocationId { get; private set; }

    public string Title { get; private set; }

    public string Address { get; private set; }

    private Location(string title, string address)
    {
        Title = title;
        Address = address;
    }

    /// <summary>
    /// Создаёт новую локацию с указанным названием и адресом.
    /// </summary>
    /// <param name="title">Название локации.</param>
    /// <param name="address">Адрес локации.</param>
    /// <returns>Новый экземпляр <see cref="Location"/>.</returns>
    /// <exception cref="DomainException">
    /// Выбрасывается, если название локации не удовлетворяет ограничениям по длине.
    /// </exception>
    public static Location Create(string title, string address)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            throw new DomainException(MaxMinMessage(
                "Название локации",  
                MinTitleLength, 
                MaxTitleLength));
        }

        return new Location(title, address);
    }

    /// <summary>
    /// Восстанавливает существующую локацию (например, из базы данных).
    /// </summary>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="title">Название локации.</param>
    /// <param name="address">Адрес локации.</param>
    /// <returns>Восстановленный экземпляр <see cref="Location"/>.</returns>
    internal static Location Restore(int locationId, string title, string address)
    {
        return new Location(title, address){ LocationId = locationId  };
    }

    private static string MaxMinMessage(object someObject, int min, int max)
        => $"{someObject} не должно быть меньше {min} или больше {max} символов";

    /// <summary>
    /// Обновляет название и адрес локации.
    /// </summary>
    /// <param name="title">Новое название локации.</param>
    /// <param name="address">Новый адрес локации.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если новое название локации не удовлетворяет ограничениям по длине.
    /// </exception>
    public void UpdateLocation(string title, string address)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            throw new DomainException(MaxMinMessage(
                "Название локации",  
                MinTitleLength, 
                MaxTitleLength));
        }
        
        Title = title;
        Address = address;
    }
}