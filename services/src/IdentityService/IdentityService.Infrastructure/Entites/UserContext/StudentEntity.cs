using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class StudentEntity : IEntity<StudentEntity, Student>
{
    private StudentEntity(Student student)
    {
        StudentId = student.StudentId;
        GroupId = student.GroupId;
    }

    internal StudentEntity() { }

    [Column("student_id")]
    public Guid StudentId;

    [ForeignKey(nameof(StudentId))]
    public UserEntity User { get; set; } = null!;

    [Column("group_id")]
    public int GroupId { get; set; }

    [ForeignKey(nameof(GroupId))]
    public GroupEntity Group { get; set; } = null!;

    public Student ToDomain()
    {
        return Student.Restore(StudentId, GroupId);
    }

    public static StudentEntity ToEntity(Student domain)
    {
        return new StudentEntity(domain);
    }
}
