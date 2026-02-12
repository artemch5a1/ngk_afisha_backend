namespace EventService.Domain.CustomExceptions;

/// <summary>
/// Ошибка получения контента (отсутствие)
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Имя не найденного объекта
    /// </summary>
    public string ObjectName;

    /// <summary>
    /// Идентификатор поиска
    /// </summary>
    public readonly object Identifier;

    public NotFoundException(string objectName, object identifier)
        : base($"Ресурс \"{objectName}\" не найден по: {identifier}")
    {
        ObjectName = objectName;
        Identifier = identifier;
    }
}
