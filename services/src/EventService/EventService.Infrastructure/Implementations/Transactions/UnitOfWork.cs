using EventService.Domain.Abstractions.Infrastructure.Transactions;
using EventService.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventService.Infrastructure.Implementations.Transactions;

public class UnitOfWork : IUnitOfWork
{
    private readonly EventServiceDbContext _db;
    
    private IDbContextTransaction? _transaction;

    public UnitOfWork(EventServiceDbContext db)
    {
        _db = db;
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started");
            
        _transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {    
        if (_transaction is null)
            throw new InvalidOperationException("Transaction not started");
        
        try
        {
            var result = await _db.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task Rollback(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;
        
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync(); 
        _transaction = null;
    }
}