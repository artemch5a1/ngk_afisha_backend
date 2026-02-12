using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.UserCases.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<List<User>>>
{
    private readonly ILogger<GetAllUsersHandler> _logger;

    private readonly IUserService _userService;

    public GetAllUsersHandler(ILogger<GetAllUsersHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<List<User>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<User> result = await _userService.GetAllUsers(cancellationToken);

            return Result<List<User>>.Success(result);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при попытке получить всех пользователей");
            return Result<List<User>>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при попытке получить всех пользователей");
            return Result<List<User>>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при попытке получить всех пользователей");
            return Result<List<User>>.Failure(ex);
        }
    }
}
