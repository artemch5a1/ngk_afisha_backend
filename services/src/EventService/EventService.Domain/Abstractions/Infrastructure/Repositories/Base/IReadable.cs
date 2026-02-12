using EventService.Domain.Contract;

namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Base;

/// <summary>
/// Интерфейс для чтения записией из хранилища
/// </summary>
/// <typeparam name="TModel">Тип сущности</typeparam>
/// <typeparam name="TId">Тип идентификатора</typeparam>
public interface IReadable<TModel, in TId>
{
    /// <summary>
    /// Получение всех записей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <param name="contract">Структура данных для получения по частям</param>
    /// <returns>Коллекция записей сущности</returns>
    Task<List<TModel>> GetAll(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Получение одной записи по id
    /// </summary>
    /// <param name="id">Идентификатор записи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Запись сущности</returns>
    Task<TModel?> GetById(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Быстрый поиск одной записи по ключу (идентификатору)
    /// </summary>
    /// <param name="id">Идентификатор записи (первичный ключ)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Запись сущности</returns>
    /// <remarks>Получает данные либо из кэша, либо по простому запросу. Без связанных сущностей</remarks>
    Task<TModel?> FindAsync(TId id, CancellationToken cancellationToken = default);
}
