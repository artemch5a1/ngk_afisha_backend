namespace IdentityService.Domain.Abstractions.Infrastructure.Utils;

/// <summary>
/// Интерфейс для хэширования пароля
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Хэширует пароль
    /// </summary>
    /// <param name="password">Изначальный вид пароля</param>
    /// <returns>Хэш от пароля</returns>
    string HashPassword(string password);

    /// <summary>
    /// Проверка соотвествия пароля хэшу
    /// </summary>
    /// <param name="password">Проверяемый пароль</param>
    /// <param name="passwordHash">Хэш пароля из хранилища</param>
    /// <returns>Буеловое значение</returns>
    bool VerifyPassword(string password, string passwordHash);
}
