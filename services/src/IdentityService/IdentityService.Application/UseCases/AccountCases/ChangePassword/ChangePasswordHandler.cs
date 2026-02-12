using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.AccountCases.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result<Guid>>
{
    private readonly IAccountService _accountService;

    private readonly ILogger<ChangePasswordHandler> _logger;

    public ChangePasswordHandler(
        IAccountService accountService,
        ILogger<ChangePasswordHandler> logger
    )
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _accountService.ChangeAccountPassword(
                request.AccountId,
                request.OldPassword,
                request.NewPassword,
                cancellationToken
            );

            return result
                ? Result<Guid>.Success(request.AccountId)
                : Result<Guid>.Failure(
                    ["Ошибка при попытке смены пароля"],
                    ApiErrorType.BadRequest
                );
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при смене пароля: {UserId}", request.AccountId);
            return Result<Guid>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(
                ex,
                "Ошибка базы данных при смене пароля: {UserId}",
                request.AccountId
            );
            return Result<Guid>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка смене пароля: {UserId}", request.AccountId);

            return Result<Guid>.Failure(ex);
        }
    }
}
