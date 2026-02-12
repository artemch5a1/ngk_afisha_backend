using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.UserCases.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<Guid>>
{
    private readonly IUserService _userService;

    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IUserService userService, ILogger<UpdateUserHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _userService.UpdateUserInfo(
                request.UserId,
                request.Surname,
                request.Name,
                request.Patronymic,
                request.DateBirth
            );

            if (result)
                return Result<Guid>.Success(request.UserId);

            return Result<Guid>.Failure(["Ошибка при обновлении данных"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(
                ex,
                "Доменная ошибка при обновлении пользователя: {UserId}",
                request.UserId
            );
            return Result<Guid>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(
                ex,
                "Ошибка базы данных при обновлении пользователя: {UserId}",
                request.UserId
            );
            return Result<Guid>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Произошла ошибка при обновлении пользователя: {UserId}",
                request.UserId
            );

            return Result<Guid>.Failure(ex);
        }
    }
}
