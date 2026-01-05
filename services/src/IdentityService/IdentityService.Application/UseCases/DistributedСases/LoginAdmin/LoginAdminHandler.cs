using IdentityService.Application.Contracts;
using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.Abstractions.Infrastructure.Providers;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.DistributedСases.LoginAdmin;

public class LoginAdminHandler : IRequestHandler<LoginAdminCommand, Result<LoginResponse>>
{
    private readonly IAccountService _accountService;

    private readonly ILogger<LoginAdminHandler> _logger;

    private readonly IAccessTokenProvider _accessTokenProvider;
    
    public LoginAdminHandler(IAccountService accountService, ILogger<LoginAdminHandler> logger, IAccessTokenProvider accessTokenProvider)
    {
        _accountService = accountService;
        _logger = logger;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<Result<LoginResponse>> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Account? account = await _accountService.LoginAdminAsync(request.Email, request.Password, cancellationToken);

            if (account is null)
                return Result<LoginResponse>.Failure(["Неправильный логин или пароль"], ApiErrorType.Unauthorized);

            string token = _accessTokenProvider.GenerateToken(account.AccountId, account.Email, account.AccountRole);

            LoginResponse response = new LoginResponse(account.AccountId, account.Email, account.AccountRole, token);

            return Result<LoginResponse>.Success(response);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при попытке логина");
            return Result<LoginResponse>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при попытке логина");
            return Result<LoginResponse>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка попытке логина");
            return Result<LoginResponse>.Failure(ex);
        }
    }
}