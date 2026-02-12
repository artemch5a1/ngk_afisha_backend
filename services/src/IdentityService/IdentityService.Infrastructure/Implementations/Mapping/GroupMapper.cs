using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Entites.UserContext;

namespace IdentityService.Infrastructure.Implementations.Mapping;

public class GroupMapper : IEntityMapper<GroupEntity, Group>
{
    public void BeforeMapping(ref Group domain, GroupEntity entity)
    {
        if (entity.Specialty is not null)
            domain.AddSpecialtyNavigation(entity.Specialty.ToDomain());
    }
}
