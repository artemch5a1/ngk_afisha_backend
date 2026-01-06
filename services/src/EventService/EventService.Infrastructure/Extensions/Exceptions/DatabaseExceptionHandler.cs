using EntityFramework.Exceptions.Common;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EventService.Infrastructure.Extensions.Exceptions;

public static class DatabaseExceptionHandler
{
    public static DatabaseException HandleException(this Exception ex)
    {
        if (ex is UniqueConstraintException pgEx1)
        {
            return HandleUniqueConstraintViolation(pgEx1);
        }

        if (ex is PostgresException exception)
        {
            return HandlePostgresException(exception);
        }

        return ex.InnerException switch
        {
            PostgresException pgEx => HandlePostgresException(pgEx),
            DbUpdateConcurrencyException => new DatabaseException(
                "Данные были изменены другим пользователем.",
                ApiErrorType.Conflict
            ),
            _ => new DatabaseException(
                "Произошла ошибка на сервере",
                ApiErrorType.InternalServerError
            ),
        };
    }

    private static DatabaseException HandlePostgresException(PostgresException pgEx)
    {
        return pgEx.SqlState switch
        {
            // Ошибка внешнего ключа (23503)
            "23503" => new DatabaseException(
                "Нарушение ссылочных ограничений",
                ApiErrorType.UnprocessableEntity
            ),

            // Ошибка проверки ограничения (23514)
            "23514" => new DatabaseException("Недопустимые данные.", ApiErrorType.UnprocessableEntity),

            // Ошибка NULL-ограничения (23502)
            "23502" => new DatabaseException(
                "Обязательные поля не заполнены.",
                ApiErrorType.UnprocessableEntity
            ),

            // Все остальные ошибки PostgreSQL
            _ => new DatabaseException("Произошла ошибка базы данных.", ApiErrorType.InternalServerError),
        };
    }

    private static DatabaseException HandleUniqueConstraintViolation(UniqueConstraintException pgEx)
    {
        return new DatabaseException(
            $"Такой {pgEx.ConstraintProperties?.FirstOrDefault()} уже существует",
            ApiErrorType.Conflict
        );
    }
}