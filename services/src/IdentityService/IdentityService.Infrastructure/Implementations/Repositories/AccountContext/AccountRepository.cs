using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.AccountContext;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.AccountContext;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.AccountContext;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Implementations.Repositories.AccountContext;

public class AccountRepository : IAccountRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly IEntityMapper<AccountEntity, Account> _accountMapper;

    private readonly ILogger<AccountRepository> _logger;
    
    public AccountRepository(
        IdentityServiceDbContext db, 
        IEntityMapper<AccountEntity, Account> accountMapper, 
        ILogger<AccountRepository> logger)
    {
        _db = db;
        _accountMapper = accountMapper;
        _logger = logger;
    }

    public async Task<List<Account>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _db.Accounts
                .Select(x => _accountMapper.ToDomain(x))
                .ToListAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка бд при получении аккаунтов");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при получении аккаунтов");
            throw ex.HandleException();
        }
    }

    public async Task<Account?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            AccountEntity? accountEntity = await _db.Accounts
                .FirstOrDefaultAsync(x => x.AccountId == id, cancellationToken);

            if (accountEntity is null)
                return null;

            return _accountMapper.ToDomain(accountEntity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка бд при получении аккаунта по id");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при получении аккаунта по id");
            throw ex.HandleException();
        }
    }

    public async Task<Account?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            AccountEntity? accountEntity = await _db.Accounts
                .FindAsync(id, cancellationToken);

            if (accountEntity is null)
                return null;

            return _accountMapper.ToDomain(accountEntity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка бд при получении аккаунта по id");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при получении аккаунта по id");
            throw ex.HandleException();
        }
    }

    public async Task<Account> Create(Account model, CancellationToken cancellationToken = default)
    {
        try
        {
            AccountEntity entity = new AccountEntity(model);

            var result = await _db.Accounts.AddAsync(entity, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            return result.Entity.ToDomain();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка бд при создании аккаунта");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при создании аккаунта");
            throw ex.HandleException();
        }
    }

    public async Task<Account?> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            AccountEntity? accountEntity = await _db.Accounts
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (accountEntity is null)
                return null;

            return _accountMapper.ToDomain(accountEntity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при получении аккаунта");
            throw ex.HandleException();
        }
    }

    public async Task<Account?> FindAdminByEmail(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            AccountEntity? accountEntity = await _db.Accounts
                .FirstOrDefaultAsync(x => x.Email == email && x.Role == (int)Role.Admin, cancellationToken);

            if (accountEntity is null)
                return null;

            return _accountMapper.ToDomain(accountEntity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при получении аккаунта");
            throw ex.HandleException();
        }
    }
    
    public async Task<Account?> FindOnlyUsersByEmail(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            AccountEntity? accountEntity = await _db.Accounts
                .FirstOrDefaultAsync(x => x.Email == email && x.Role != (int)Role.Admin, cancellationToken);

            if (accountEntity is null)
                return null;

            return _accountMapper.ToDomain(accountEntity);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при получении аккаунта");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(Account model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.Accounts
                .Where(x => x.AccountId == model.AccountId)
                .ExecuteUpdateAsync(
                    x => x
                        .SetProperty(i => i.PasswordHash, i => model.PasswordHash),
                    cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка бд при обновлении аккаунта");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка бд при обновлении аккаунта");
            throw ex.HandleException();
        }
    }
}