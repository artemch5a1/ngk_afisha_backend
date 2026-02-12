using IdentityService.Domain.Enums;

namespace IdentityService.Domain.CustomExceptions;

/// <summary>
/// Ошибка базы данных
/// </summary>
public class DatabaseException : Exception
{
    /// <summary>
    /// Тип ошибки
    /// </summary>
    public ApiErrorType ErrorType { get; private set; }

    public DatabaseException(string message, ApiErrorType errorType)
        : base(message)
    {
        ErrorType = errorType;
    }
}
