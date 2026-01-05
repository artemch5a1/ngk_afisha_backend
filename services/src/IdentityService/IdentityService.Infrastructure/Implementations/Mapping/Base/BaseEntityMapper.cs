using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Abstractions.Infrastructure.Mapping;

namespace IdentityService.Infrastructure.Implementations.Mapping.Base;

public class BaseEntityMapper<TEntity, TDomain> : IEntityMapper<TEntity, TDomain> 
    where TEntity : IEntity<TEntity, TDomain>
{
    public void BeforeMapping(ref TDomain domain, TEntity entity){ }
}