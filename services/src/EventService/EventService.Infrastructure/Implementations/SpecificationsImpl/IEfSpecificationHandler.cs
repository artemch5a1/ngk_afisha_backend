using System.Linq.Expressions;

namespace EventService.Infrastructure.Implementations.SpecificationsImpl;

public interface IEfSpecificationHandler<out TDomainModel, TEntity>
{
    Expression<Func<TEntity, bool>> Apply();

    string Name { get; }
}
