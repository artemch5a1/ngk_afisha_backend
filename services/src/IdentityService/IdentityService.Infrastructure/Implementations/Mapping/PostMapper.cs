using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Entites.UserContext;

namespace IdentityService.Infrastructure.Implementations.Mapping;

public class PostMapper : IEntityMapper<PostEntity, Post>
{
    public void BeforeMapping(ref Post domain, PostEntity entity)
    {
        if(entity.Department is not null)
            AddDepartmentNavigation(ref domain, entity.Department);
    }

    private void AddDepartmentNavigation(ref Post domain, DepartmentEntity entity)
    {
        Department department = entity.ToDomain();
        
        domain.AddDepartmentNavigation(department);
    }
}