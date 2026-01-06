using EventService.Domain.CustomExceptions;
using EventService.Domain.Enums;

namespace EventService.Domain.Result;

/// <summary>
/// Представляет результат выполнения операции, который может быть успешным или завершиться ошибкой.
/// </summary>
/// <typeparam name="T">Тип значения, возвращаемого при успешном выполнении.</typeparam>
public sealed class Result<T>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">Значение результата при успешном выполнении.</param>
    /// <param name="isSuccess">Флаг, указывающий, успешна ли операция.</param>
    /// <param name="errorMessages">Массив сообщений об ошибках.</param>
    /// <param name="errorType">Тип ошибки, если операция завершилась неудачей.</param>
    private Result(T? value, bool isSuccess, string[] errorMessages, ApiErrorType errorType)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        ErrorType = errorType;
    }

    /// <summary>
    /// Значение результата операции. Имеет значение <c>null</c>, если операция завершилась неудачей.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Возвращает <c>true</c>, если операция выполнена успешно; иначе <c>false</c>.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Тип ошибки операции.
    /// </summary>
    public ApiErrorType ErrorType { get; }

    /// <summary>
    /// Массив сообщений об ошибках, если операция завершилась неудачей.
    /// </summary>
    public string[] ErrorMessages { get; }

    /// <summary>
    /// Возвращает первое сообщение об ошибке или пустую строку, если ошибок нет.
    /// </summary>
    public string FirstMessage => ErrorMessages.FirstOrDefault() ?? string.Empty;
    
    
    /// <summary>
    /// Создаёт успешный результат с указанным значением.
    /// </summary>
    /// <param name="value">Значение результата.</param>
    /// <returns>Экземпляр <see cref="Result{T}"/> с успешным статусом.</returns>
    public static Result<T> Success(T value) => new Result<T>(value, true, [], ApiErrorType.Ok);

    /// <summary>
    /// Создаёт результат с ошибкой и одним сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="errorType">Тип ошибки.</param>
    /// <returns>Экземпляр <see cref="Result{T}"/> с ошибкой.</returns>
    public static Result<T> Failure(string message, ApiErrorType errorType)
        => new Result<T>(default, false, [message], errorType);

    /// <summary>
    /// Создаёт результат с ошибкой и массивом сообщений.
    /// </summary>
    /// <param name="messages">Массив сообщений об ошибках.</param>
    /// <param name="errorType">Тип ошибки.</param>
    /// <returns>Экземпляр <see cref="Result{T}"/> с ошибкой.</returns>
    public static Result<T> Failure(string[] messages, ApiErrorType errorType) 
        => new Result<T>(default, false, messages, errorType);

    /// <summary>
    /// Создаёт результат с ошибкой на основе исключения.
    /// </summary>
    /// <param name="ex">Исключение, которое вызвало ошибку.</param>
    /// <returns>Экземпляр <see cref="Result{T}"/> с ошибкой.</returns>
    public static Result<T> Failure(Exception ex)
    {
        return ex switch
        {
            DomainException domainException => new Result<T>(default, false, [domainException.Message],
                ApiErrorType.BadRequest),
            
            DatabaseException databaseException => new Result<T>(default, false, [databaseException.Message],
                databaseException.ErrorType),
            
            NotFoundException notFoundException => new Result<T>(default, false, [notFoundException.Message],
                ApiErrorType.NotFound),
            
            _ => new Result<T>(default, false, [ex.Message], ApiErrorType.InternalServerError)
        };
    }
}