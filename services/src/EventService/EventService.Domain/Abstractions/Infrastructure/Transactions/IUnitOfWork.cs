namespace EventService.Domain.Abstractions.Infrastructure.Transactions;

/// <summary>
/// Интерфейс для построения транзакций при работе с разными контекстами
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Применить изменения к базе данных
    /// </summary>
    /// <returns>Количество измененых строк</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Откат транзакции
    /// </summary>
    Task Rollback(CancellationToken cancellationToken = default);
}
