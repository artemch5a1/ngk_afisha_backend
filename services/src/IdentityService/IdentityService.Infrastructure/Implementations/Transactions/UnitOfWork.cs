using IdentityService.Domain.Abstractions.Infrastructure.Transactions;
using IdentityService.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityService.Infrastructure.Implementations.Transactions;

public class UnitOfWork : IUnitOfWork
{
    private readonly IdentityServiceDbContext _db;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(IdentityServiceDbContext db)
    {
        _db = db;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started");

        _transaction = await _db.Database.BeginTransactionAsync();
    }

    public async Task<int> CommitAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("Transaction not started");

        try
        {
            var result = await _db.SaveChangesAsync();
            await _transaction.CommitAsync();
            return result;
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task Rollback()
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
