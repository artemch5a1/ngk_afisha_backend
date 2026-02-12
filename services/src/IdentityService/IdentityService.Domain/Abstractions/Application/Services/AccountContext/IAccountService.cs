using IdentityService.Domain.Models.AccountContext;

namespace IdentityService.Domain.Abstractions.Application.Services.AccountContext;

public interface IAccountService
{
    /// <summary>
    /// Логин под именем пользователя или публикатора
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="password">Пароль</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Аккаунт при удачном логине, null при неверных учетных данных</returns>
    Task<Account?> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Логин под администратором системы
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="password">Пароль</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Аккаунт при удачном логине, null при неверных учетных данных</returns>
    Task<Account?> LoginAdminAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
    );

    Task<List<Account>> GetAllAccounts(CancellationToken ct = default);

    Task<Account?> GetAccountById(Guid accountId, CancellationToken ct = default);

    Task<Account> CreateStudentAccount(
        string email,
        string password,
        CancellationToken ct = default
    );

    Task<Account> CreatePublisherAccount(
        string email,
        string password,
        CancellationToken ct = default
    );

    Task<bool> ChangeAccountPassword(
        Guid accountId,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default
    );
}
