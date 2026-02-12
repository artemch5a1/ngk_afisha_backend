using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Result;

public sealed class Result<T>
{
    private Result(T? value, bool isSuccess, string[] errorMessages, ApiErrorType errorType)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        ErrorType = errorType;
    }

    public T? Value { get; }

    public bool IsSuccess { get; }

    public ApiErrorType ErrorType { get; }

    public string[] ErrorMessages { get; }

    public string FirstMessage => ErrorMessages.FirstOrDefault() ?? string.Empty;

    public static Result<T> Success(T value) => new Result<T>(value, true, [], ApiErrorType.Ok);

    public static Result<T> Failure(string message, ApiErrorType errorType) =>
        new Result<T>(default, false, [message], errorType);

    public static Result<T> Failure(string[] messages, ApiErrorType errorType) =>
        new Result<T>(default, false, messages, errorType);

    public static Result<T> Failure(Exception ex)
    {
        return ex switch
        {
            DomainException domainException => new Result<T>(
                default,
                false,
                [domainException.Message],
                ApiErrorType.BadRequest
            ),

            DatabaseException databaseException => new Result<T>(
                default,
                false,
                [databaseException.Message],
                databaseException.ErrorType
            ),

            NotFoundException notFoundException => new Result<T>(
                default,
                false,
                [notFoundException.Message],
                ApiErrorType.NotFound
            ),

            _ => new Result<T>(default, false, [ex.Message], ApiErrorType.InternalServerError),
        };
    }
}
