using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Entites.UserContext;

namespace IdentityService.Infrastructure.Implementations.Mapping;

public class PublisherMapper : IEntityMapper<PublisherEntity, Publisher>
{
    public void BeforeMapping(ref Publisher domain, PublisherEntity entity)
    {
        if (entity.User is not null)
            AddUserNavigation(ref domain, entity.User);

        if (entity.Post is not null)
            AddPostNavigation(ref domain, entity.Post);
    }

    private void AddUserNavigation(ref Publisher domain, UserEntity entity)
    {
        User user = entity.ToDomain();

        domain.AddUserNavigation(user);
    }

    private void AddPostNavigation(ref Publisher domain, PostEntity entity)
    {
        Post post = entity.ToDomain();

        if (entity.Department is not null)
            AddDepartmentNavigation(ref post, entity.Department);

        domain.AddPostNavigation(post);
    }

    private void AddDepartmentNavigation(ref Post domain, DepartmentEntity entity)
    {
        Department department = entity.ToDomain();

        domain.AddDepartmentNavigation(department);
    }
}
