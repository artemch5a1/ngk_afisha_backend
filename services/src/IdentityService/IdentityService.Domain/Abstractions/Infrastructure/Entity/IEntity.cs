namespace IdentityService.Domain.Abstractions.Infrastructure.Entity;

/// <summary>
/// Базовый интерфейс сущности. Связывает сущность с доменной моделью 1:1
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TDomain">Тип доменной модели</typeparam>
public interface IEntity<out TEntity, TDomain>
{
    /// <summary>
    /// Возвращает домен на основе сущности
    /// </summary>
    /// <returns>Доменная модель</returns>
    /// <remarks>Не несет отвественность за восстановление навигационных свойтсв для избежания цикличных зависимостей.</remarks>
    TDomain ToDomain();

    /// <summary>
    /// Фабрика для создания сущностей из домена
    /// </summary>
    /// <param name="domain">Доменная модель</param>
    /// <returns>Сущность</returns>
    /// <remarks>Гарантирует целостность агрегата (все вложенные агрегаты должны быть восстановлены)</remarks>
    static abstract TEntity ToEntity(TDomain domain);
}
