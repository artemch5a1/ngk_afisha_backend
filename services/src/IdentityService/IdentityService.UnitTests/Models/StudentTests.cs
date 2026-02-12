using FluentAssertions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class StudentTests
{
    [Fact]
    public void Create_ShouldReturnStudentWithCorrectData()
    {
        // Arrange
        Guid studentId = Guid.NewGuid();
        int groupId = 101;

        // Act
        var student = Student.Create(studentId, groupId);

        // Assert
        student.Should().NotBeNull();
        student.StudentId.Should().Be(studentId);
        student.GroupId.Should().Be(groupId);
        student.Group.Should().BeNull();
        student.User.Should().BeNull();
    }

    [Fact]
    public void Restore_ShouldReturnStudentWithCorrectData()
    {
        // Arrange
        Guid studentId = Guid.NewGuid();
        int groupId = 202;

        // Act
        var student = Student.Restore(studentId, groupId);

        // Assert
        student.StudentId.Should().Be(studentId);
        student.GroupId.Should().Be(groupId);
    }

    [Fact]
    public void UpdateStudent_ShouldChangeGroupId()
    {
        // Arrange
        var student = Student.Create(Guid.NewGuid(), 10);

        // Act
        student.UpdateStudent(15);

        // Assert
        student.GroupId.Should().Be(15);
    }
}
