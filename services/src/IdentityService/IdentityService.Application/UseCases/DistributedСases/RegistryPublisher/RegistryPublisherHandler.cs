using IdentityService.Application.Contracts;
using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Transactions;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.DistributedСases.RegistryPublisher;

public class RegistryPublisherHandler : IRequestHandler<RegistryPublisherCommand, Result<AccountCreated>>
{
    private readonly IAccountService _accountService;

    private readonly IUserService _userService;
    
    private readonly ILogger<RegistryPublisherHandler> _logger;

    private readonly IUnitOfWork _unitOfWork;


    public RegistryPublisherHandler(
        IAccountService accountService, 
        IUserService userService, 
        IUnitOfWork unitOfWork, 
        ILogger<RegistryPublisherHandler> logger)
    {
        _accountService = accountService;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<AccountCreated>> Handle(RegistryPublisherCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            Account accountResult =
                await _accountService.CreatePublisherAccount(request.Email, request.Password, cancellationToken);

            User userResult =
                await _userService.CreatePublisher(
                    accountResult.AccountId,
                    request.Surname,
                    request.Name,
                    request.Patronymic,
                    request.DateBirth,
                    request.PostId,
                    cancellationToken);

            await _unitOfWork.CommitAsync();
            
            return Result<AccountCreated>
                .Success(
                    new AccountCreated(
                        accountResult.AccountId,
                        accountResult.Email,
                        accountResult.AccountRole
                    ));
        }
        catch (DatabaseException ex)
        {
            await _unitOfWork.Rollback();
            
            _logger.LogWarning(ex, "Ошибка базы данных при регистрации пользователя");

            return Result<AccountCreated>.Failure(ex);
        }
        catch (DomainException ex)
        {
            await _unitOfWork.Rollback();
            
            _logger.LogWarning(ex, "Доменная ошибка при регистрации пользователя");

            return Result<AccountCreated>.Failure(ex);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            
            _logger.LogError(ex, "Ошибка при регистрации пользователя");

            return Result<AccountCreated>.Failure(ex);
        }
    }
}