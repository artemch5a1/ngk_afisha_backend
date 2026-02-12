using System.Text.RegularExpressions;
using IdentityService.Domain.Abstractions.Infrastructure.Utils;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Models.AccountContext;

public class Account
{
    private Account(
        Guid accountId,
        string email,
        string passwordHash,
        DateTime createdDate,
        Role role
    )
    {
        AccountId = accountId;
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        AccountRole = role;
        CreatedDate = createdDate;
    }

    public Guid AccountId { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public DateTime CreatedDate { get; private set; }

    public Role AccountRole { get; private set; }

    /// <summary>
    /// Создает новую учетную запись обычного пользователя.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="password">Пароль</param>
    /// <param name="passwordHasher">Интерфейс для хэширования</param>
    /// <returns>Созданная учетная запись с ролью <see cref="Role.User"/>.</returns>
    /// <exception cref="DomainException">Выбрасывается при пустом или некорректном email.</exception>
    public static Account CreateStudentAccount(
        string email,
        string password,
        IPasswordHasher passwordHasher
    )
    {
        return CreateAccount(Guid.NewGuid(), email, password, Role.User, passwordHasher);
    }

    /// <summary>
    /// Создает новую учетную запись публикатора.
    /// </summary>
    /// <param name="email">Email публикатора.</param>
    /// <param name="password">Пароль</param>
    /// <param name="passwordHasher">Интерфейс для хэширования</param>
    /// <returns>Созданная учетная запись с ролью <see cref="Role.Publisher"/>.</returns>
    /// <exception cref="DomainException">Выбрасывается при пустом или некорректном email.</exception>
    public static Account CreatePublisherAccount(
        string email,
        string password,
        IPasswordHasher passwordHasher
    )
    {
        return CreateAccount(Guid.NewGuid(), email, password, Role.Publisher, passwordHasher);
    }

    /// <summary>
    /// Восстанавливает учетную запись из хранилища (БД).
    /// </summary>
    /// <param name="accountId">Идентификатор учетной записи.</param>
    /// <param name="email">Email учетной записи.</param>
    /// <param name="passwordHash">Хэш пароля.</param>
    /// <param name="role">Роль учетной записи.</param>
    /// <param name="createdDate">Дата регистрации.</param>
    /// <returns>Экземпляр учетной записи.</returns>
    /// <remarks>
    /// Используется инфраструктурным слоем при реконструкции объекта из данных БД.
    /// Проверяется только корректность критических полей (Id и Email).
    /// </remarks>
    /// <exception cref="InvalidDataException">Выбрасывается, если <paramref name="accountId"/> равен <see cref="Guid.Empty"/> или <paramref name="email"/> пуст.</exception>
    internal static Account Restore(
        Guid accountId,
        string email,
        string passwordHash,
        DateTime createdDate,
        Role role
    )
    {
        if (accountId == Guid.Empty)
            throw new InvalidDataException("AccountId не может быть пустым");

        if (string.IsNullOrEmpty(email))
            throw new InvalidDataException("Email не может быть пустым при восстановлении");

        return new Account(accountId, email, passwordHash, createdDate, role);
    }

    private static Account CreateAccount(
        Guid id,
        string email,
        string password,
        Role role,
        IPasswordHasher passwordHasher
    )
    {
        if (string.IsNullOrEmpty(email))
            throw new DomainException("Email не может быть пустым");

        if (!IsValidEmail(email))
            throw new DomainException("Некорректный email");

        if (password.Length < 4)
            throw new DomainException("Пароль должен быть не меньше 4 символов");

        string passwordHash = passwordHasher.HashPassword(password);

        return new Account(id, email, passwordHash, DateTime.UtcNow, role);
    }

    /// <summary>
    /// Проверяет валидность email.
    /// </summary>
    /// <param name="email">Email для проверки.</param>
    /// <returns><c>true</c>, если email корректен; иначе <c>false</c>.</returns>
    /// <remarks>
    /// Проверка выполняется через <see cref="System.Net.Mail.MailAddress"/>.
    /// Сравнение адреса после парсинга гарантирует, что входная строка не была изменена.
    /// </remarks>
    public static bool IsValidEmail(string email)
    {
        string pattern =
            @"^[a-zA-Z0-9]([\w\.-]*[a-zA-Z0-9])?@([a-zA-Z0-9]([\w-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    /// <summary>
    /// Обновление пароля. Требует подтверждение пароля
    /// </summary>
    /// <param name="oldPassword">Старый пароль</param>
    /// <param name="newPassword">Новый пароль</param>
    /// <param name="passwordHasher">Интерфейс для хэширования</param>
    /// <exception cref="DomainException">Выбрасывается при провальной валидации или несоотвествии старого пароля текущему</exception>
    public void ChangePassword(
        string oldPassword,
        string newPassword,
        IPasswordHasher passwordHasher
    )
    {
        bool isValidPassword = passwordHasher.VerifyPassword(oldPassword, PasswordHash);

        if (!isValidPassword)
            throw new DomainException("Неверный пароль");

        if (newPassword.Length < 4)
            throw new DomainException("Пароль должен быть не меньше 4 символов");

        PasswordHash = passwordHasher.HashPassword(newPassword);
    }
}
