using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Abstractions.Infrastructure.Providers;

/// <summary>
/// Интерфейс для генерации и верификации токена досутпа
/// </summary>
public interface IAccessTokenProvider
{
    /// <summary>
    /// Генерирует токен на основе id аккаунта, email и роли
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="email">Почта</param>
    /// <param name="role">Роль в системе</param>
    /// <returns>Токен в виде строки</returns>
    string GenerateToken(Guid accountId, string email, Role role);

    /// <summary>
    /// Валидирует токен на публичным ключом
    /// </summary>
    /// <param name="token">Токен в виде строки</param>
    /// <returns>Булевое значение</returns>
    bool ValidateToken(string token);
}
