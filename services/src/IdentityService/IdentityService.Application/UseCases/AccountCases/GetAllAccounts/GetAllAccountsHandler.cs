using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.AccountCases.GetAllAccounts;

public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, Result<List<Account>>>
{
    private readonly ILogger<GetAllAccountsHandler> _logger;

    private readonly IAccountService _accountService;
    
    public GetAllAccountsHandler(
        ILogger<GetAllAccountsHandler> logger, 
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }
    
    public async Task<Result<List<Account>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Account> result = await _accountService.GetAllAccounts(cancellationToken);

            return Result<List<Account>>.Success(result);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при попытке получить все аккаунты");
            return Result<List<Account>>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при попытке получить все аккаунты");
            return Result<List<Account>>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при попытке получить все аккаунты");
            return Result<List<Account>>.Failure(ex);
        }
    }
}