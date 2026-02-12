namespace EventService.Domain.Enums;

public enum ApiErrorType
{
    Ok = 200,

    // Ошибки клиента (4xx)
    BadRequest = 400, // Неверные данные/валидация
    Unauthorized = 401, // Нет токена, пользователь не аутентифицирован
    Forbidden = 403, // Нет прав доступа
    NotFound = 404, // Ресурс не найден
    Conflict = 409, // Конфликт (например, дубликат данных)
    UnprocessableEntity = 422, // Данные корректны синтаксически, но невалидны по бизнес-логике
    TooManyRequests = 429, // Превышен лимит запросов

    // Ошибки сервера (5xx)
    InternalServerError = 500, // Общая ошибка на сервере
    ServiceUnavailable = 503, // Внешний сервис/БД недоступны
    GatewayTimeout = 504, // Таймаут ожидания ответа
}
