using System.Linq.Expressions;
using EventService.Domain.Abstractions.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace EventService.Infrastructure.Implementations.SpecificationsImpl;

public class EfSpecificationMapper
{
    private readonly IServiceProvider _serviceProvider;

    public EfSpecificationMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Expression<Func<TEntity, bool>> ResolveEfSpecification<TDomain, TEntity>(ISpecification<TDomain> domainSpecification)
    {
        IEfSpecificationHandler<TDomain, TEntity> handler = _serviceProvider
            .GetServices<IEfSpecificationHandler<TDomain, TEntity>>()
            .First(x => x.Name == domainSpecification.Name);

        return handler.Apply();
    }
}