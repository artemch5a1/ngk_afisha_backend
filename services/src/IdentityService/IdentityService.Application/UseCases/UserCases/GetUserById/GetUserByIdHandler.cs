using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.UserCases.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly ILogger<GetUserByIdHandler> _logger;

    private readonly IUserService _userService;

    public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<User>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            User? result = await _userService.GetUserById(request.UserId, cancellationToken);

            if (result is null)
                return Result<User>.Failure("Пользователь не найден", ApiErrorType.NotFound);

            return Result<User>.Success(result);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при попытке получить пользователя по id");
            return Result<User>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при попытке получить пользователя по id");
            return Result<User>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при попытке получить пользователя по id");
            return Result<User>.Failure(ex);
        }
    }
}
