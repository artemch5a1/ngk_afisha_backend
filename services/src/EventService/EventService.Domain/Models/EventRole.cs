using EventService.Domain.CustomExceptions;

namespace EventService.Domain.Models;

/// <summary>
/// Представляет роль в мероприятии с названием и описанием.
/// </summary>
public class EventRole
{
    private const int MaxTitleLength = 35;
    
    private const int MinTitleLength = 2;
    
    private const int MaxDescLength = 75;
    
    private const int MinDescLength = 15;
    
    public int EventRoleId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }
    
    private EventRole(string title, string description)
    {
        Title = title;
        Description = description;
    }

    /// <summary>
    /// Создаёт новую роль мероприятия с указанным названием и описанием.
    /// </summary>
    /// <param name="title">Название роли.</param>
    /// <param name="description">Описание роли.</param>
    /// <returns>Новый экземпляр <see cref="EventRole"/>.</returns>
    /// <exception cref="DomainException">
    /// Выбрасывается, если название или описание не удовлетворяют ограничениям по длине.
    /// </exception>
    public static EventRole Create(string title, string description)
    {
        ExecuteValidation(title, description);

        return new EventRole(title, description);
    }

    /// <summary>
    /// Восстанавливает существующую роль мероприятия (например, из базы данных).
    /// </summary>
    /// <param name="eventRoleId">Идентификатор роли.</param>
    /// <param name="title">Название роли.</param>
    /// <param name="description">Описание роли.</param>
    /// <returns>Восстановленный экземпляр <see cref="EventRole"/>.</returns>

    internal static EventRole Restore(int eventRoleId, string title, string description)
    {
        return new EventRole(title, description) { EventRoleId = eventRoleId };
    }

    private static void ExecuteValidation(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
            throw new DomainException(MaxMinMessage("Название роли", MinTitleLength, MaxTitleLength));
        
        if(string.IsNullOrWhiteSpace(description) || description.Length < MinDescLength || description.Length > MaxDescLength)
            throw new DomainException(MaxMinMessage("Описание роли", MinDescLength, MaxDescLength));
    }
    
    private static string MaxMinMessage(object someObject, int min, int max)
        => $"{someObject} не должно быть меньше {min} или больше {max} символов";

    /// <summary>
    /// Обновляет название и описание роли.
    /// </summary>
    /// <param name="title">Новое название роли.</param>
    /// <param name="description">Новое описание роли.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если новое название или описание не удовлетворяют ограничениям по длине.
    /// </exception>
    public void UpdateRole(string title, string description)
    {
        ExecuteValidation(title, description);

        Title = title;
        Description = description;
    }
}