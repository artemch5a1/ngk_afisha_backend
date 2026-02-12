namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;

/// <summary>
/// Интерфейс для обновления записи
/// </summary>
/// <typeparam name="TModel">Тип сущности</typeparam>
public interface IUpdatable<in TModel>
{
    /// <summary>
    /// Обновление записи
    /// </summary>
    /// <param name="model">Обновляемая запись (объект сущности)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Булевое значение (обновлена/не обновлена)</returns>
    /// <remarks>Обновляет только саму сущность без вложенных агрегатов</remarks>
    Task<bool> Update(TModel model, CancellationToken cancellationToken = default);
}
