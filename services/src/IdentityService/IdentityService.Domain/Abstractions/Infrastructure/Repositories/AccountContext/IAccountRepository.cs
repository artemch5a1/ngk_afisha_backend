using IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;
using IdentityService.Domain.Models.AccountContext;

namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.AccountContext;

public interface IAccountRepository
    : IReadable<Account, Guid>,
        IWritable<Account, Guid>,
        IUpdatable<Account>
{
    /// <summary>
    /// Ищет любой аккаунт по email
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Аккаунт либо null если его нет</returns>
    Task<Account?> FindByEmail(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ищет аккаунт по email, только тот, у которого роль администратора
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Аккаунт либо null если его нет</returns>
    Task<Account?> FindAdminByEmail(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ищет аккаунт по email, только тот, у которого роль НЕ администратора
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Аккаунт либо null если его нет</returns>
    Task<Account?> FindOnlyUsersByEmail(
        string email,
        CancellationToken cancellationToken = default
    );
}
