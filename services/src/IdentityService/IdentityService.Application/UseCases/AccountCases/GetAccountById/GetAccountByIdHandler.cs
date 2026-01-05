using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.AccountCases.GetAccountById;

public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, Result<Account>>
{
    private readonly ILogger<GetAccountByIdHandler> _logger;

    private readonly IAccountService _accountService;
    
    public GetAccountByIdHandler(
        ILogger<GetAccountByIdHandler> logger, 
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }
    
    public async Task<Result<Account>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Account? account = await _accountService.GetAccountById(request.AccountId, cancellationToken);

            if (account is null)
                return Result<Account>.Failure("Аккаунт не найден", ApiErrorType.NotFound);

            return Result<Account>.Success(account);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при попытке получить аккаунт по id");
            return Result<Account>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при попытке получить аккаунт по id");
            return Result<Account>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при попытке получить аккаунт по id");
            return Result<Account>.Failure(ex);
        }
    }
}