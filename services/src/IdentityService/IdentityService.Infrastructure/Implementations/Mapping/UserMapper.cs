using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Entites.UserContext;

namespace IdentityService.Infrastructure.Implementations.Mapping;

public class UserMapper : IEntityMapper<UserEntity, User>
{
    public void BeforeMapping(ref User domain, UserEntity entity)
    {
        if (entity.StudentProfile is not null)
            AddStudentNavigation(ref domain, entity.StudentProfile);

        if (entity.PublisherProfile is not null)
            AddPublisherNavigation(ref domain, entity.PublisherProfile);
    }

    private void AddPublisherNavigation(ref User user, PublisherEntity publisherEntity)
    {
        Publisher publisher = publisherEntity.ToDomain();

        if (publisherEntity.Post is not null)
            AddPostNavigation(ref publisher, publisherEntity.Post);

        user.AddPublisherProfile(publisher);
    }

    private void AddPostNavigation(ref Publisher publisher, PostEntity postEntity)
    {
        Post post = postEntity.ToDomain();

        if (postEntity.Department is not null)
            post.AddDepartmentNavigation(postEntity.Department.ToDomain());

        publisher.AddPostNavigation(post);
    }

    private void AddStudentNavigation(ref User user, StudentEntity studentEntity)
    {
        Student student = studentEntity.ToDomain();

        if (studentEntity.Group is not null)
            AddGroupNavigation(ref student, studentEntity.Group);

        user.AddStudentProfile(student);
    }

    private void AddGroupNavigation(ref Student student, GroupEntity groupEntity)
    {
        Group group = groupEntity.ToDomain();

        if (groupEntity.Specialty is not null)
            group.AddSpecialtyNavigation(groupEntity.Specialty.ToDomain());

        student.AddGroupNavigation(group);
    }
}
