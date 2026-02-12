namespace EventService.Domain.Abstractions.Infrastructure.Repositories.Base;

/// <summary>
/// Интерфейс для удаления записи из хранилища
/// </summary>
/// <typeparam name="TId">Тип идентификатора</typeparam>
public interface IDeletable<in TId>
{
    /// <summary>
    /// Удаляет запись
    /// </summary>
    /// <param name="id">Id записи из хранилища</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Буеловое значение (удален/не удален)</returns>
    Task<bool> Delete(TId id, CancellationToken cancellationToken = default);
}
