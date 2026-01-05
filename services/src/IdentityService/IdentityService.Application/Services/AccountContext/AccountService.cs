using IdentityService.Domain.Abstractions.Application.Services.AccountContext;
using IdentityService.Domain.Abstractions.Infrastructure.Utils;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.AccountContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.AccountContext;

namespace IdentityService.Application.Services.AccountContext;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    private readonly IPasswordHasher _passwordHasher;
    
    public AccountService(
        IAccountRepository accountRepository, 
        IPasswordHasher passwordHasher)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Account?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        Account? account = await _accountRepository.FindOnlyUsersByEmail(email, cancellationToken);

        return VerifyPassword(password, account) ? account : null;
    }

    public async Task<Account?> LoginAdminAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        Account? account = await _accountRepository.FindAdminByEmail(email, cancellationToken);

        return VerifyPassword(password, account) ? account : null;
    }

    public async Task<List<Account>> GetAllAccounts(CancellationToken ct = default)
    {
        return await _accountRepository.GetAll(ct);
    }
    
    public async Task<Account?> GetAccountById(Guid accountId, CancellationToken ct = default)
    {
        return await _accountRepository.GetById(accountId, ct);
    }

    public async Task<Account> CreateStudentAccount(string email, string password, CancellationToken ct = default)
    {
        Account account = Account.CreateStudentAccount(email, password, _passwordHasher);

        Account createdAccount = await _accountRepository.Create(account, ct);
            
        return createdAccount;
    }

    public async Task<Account> CreatePublisherAccount(string email, string password, CancellationToken ct = default)
    {
        Account account = Account.CreatePublisherAccount(email, password, _passwordHasher);

        Account createdAccount = await _accountRepository.Create(account, ct);
            
        return createdAccount;
    }

    public async Task<bool> ChangeAccountPassword(
        Guid accountId, 
        string oldPassword, 
        string newPassword, 
        CancellationToken cancellationToken = default)
    {
        Account? account = await _accountRepository.FindAsync(accountId, cancellationToken);

        if (account is null)
            throw new NotFoundException("Аккаунт", accountId);
        
        account.ChangePassword(oldPassword, newPassword, _passwordHasher);

        return await _accountRepository.Update(account, cancellationToken);
    }

    private bool VerifyPassword(string password, Account? account)
    {
        if (account is null)
            return false;

        return _passwordHasher.VerifyPassword(password, account.PasswordHash);
    }
}