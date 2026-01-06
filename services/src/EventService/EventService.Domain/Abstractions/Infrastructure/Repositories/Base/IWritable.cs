namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Base;

/// <summary>
/// Интерфейс для обновления записи
/// </summary>
/// <typeparam name="TModel">Тип сущности</typeparam>
/// <typeparam name="TId">Тип Идентификатора</typeparam>
public interface IWritable<TModel, TId>
{
    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="model">Добавляемая запись (объект сущности)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданный объект</returns>
    Task<TModel> Create(TModel model, CancellationToken cancellationToken = default);
}