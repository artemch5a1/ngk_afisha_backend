using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Entites.UserContext;

namespace IdentityService.Infrastructure.Implementations.Mapping;

public class StudentMapper : IEntityMapper<StudentEntity, Student>
{
    public void BeforeMapping(ref Student domain, StudentEntity entity)
    {
        if (entity.User is not null)
            AddUserNavigation(ref domain, entity.User);

        if (entity.Group is not null)
            AddGroupNavigation(ref domain, entity.Group);
    }

    private void AddUserNavigation(ref Student domain, UserEntity user)
    {
        domain.AddUserNavigation(user.ToDomain());
    }

    private void AddGroupNavigation(ref Student domain, GroupEntity group)
    {
        Group groupDomain = group.ToDomain();

        if (group.Specialty is not null)
            AddSpecialityNavigation(ref groupDomain, group.Specialty);

        domain.AddGroupNavigation(groupDomain);
    }

    private void AddSpecialityNavigation(ref Group domain, SpecialtyEntity specialty)
    {
        domain.AddSpecialtyNavigation(specialty.ToDomain());
    }
}
