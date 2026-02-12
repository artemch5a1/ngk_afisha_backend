using FluentAssertions;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class UserTests
{
    [Fact]
    public void CreateStudent_ShouldCreateUserWithStudentProfile()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        int groupId = 101;
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20));

        // Act
        var user = User.CreateStudent(userId, "Иванов", "Иван", "Иванович", birthDate, groupId);

        // Assert
        user.Should().NotBeNull();
        user.StudentProfile.Should().NotBeNull();
        user.PublisherProfile.Should().BeNull();
        user.Surname.Should().Be("Иванов");
        user.StudentProfile!.GroupId.Should().Be(groupId);
    }

    [Fact]
    public void CreatePublisher_ShouldCreateUserWithPublisherProfile()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        int postId = 7;
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));

        // Act
        var user = User.CreatePublisher(userId, "Петров", "Пётр", null, birthDate, postId);

        // Assert
        user.Should().NotBeNull();
        user.PublisherProfile.Should().NotBeNull();
        user.StudentProfile.Should().BeNull();
        user.PublisherProfile!.PostId.Should().Be(postId);
    }

    [Fact]
    public void CreateUser_ShouldThrow_WhenUserTooYoung()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10)); // младше 14 лет

        // Act
        Action act = () => User.CreateStudent(userId, "Иванов", "Иван", "Иванович", birthDate, 1);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не младше*");
    }

    [Fact]
    public void CreateUser_ShouldThrow_WhenUserTooOld()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-120));

        // Act
        Action act = () => User.CreateStudent(userId, "Иванов", "Иван", "Иванович", birthDate, 1);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Некорректная дата рождения*");
    }

    [Fact]
    public void Restore_ShouldCreateUserWithoutValidation()
    {
        // Arrange
        Guid userId = Guid.NewGuid();

        // Act
        var user = User.Restore(userId, "Иванов", "Иван", null, new DateOnly(2000, 1, 1));

        // Assert
        user.UserId.Should().Be(userId);
        user.Surname.Should().Be("Иванов");
    }

    [Fact]
    public void Restore_ShouldThrow_WhenUserIdEmpty()
    {
        // Act
        Action act = () =>
            User.Restore(Guid.Empty, "Иванов", "Иван", null, new DateOnly(2000, 1, 1));

        // Assert
        act.Should().Throw<InvalidDataException>();
    }

    [Fact]
    public void UpdateFields_ShouldChangeUserData()
    {
        // Arrange
        var user = User.CreatePublisher(
            Guid.NewGuid(),
            "Петров",
            "Пётр",
            "Иванович",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-40)),
            2
        );

        // Act
        user.UpdateFields(
            "Сидоров",
            "Сидор",
            null,
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25))
        );

        // Assert
        user.Surname.Should().Be("Сидоров");
        user.Name.Should().Be("Сидор");
        user.Patronymic.Should().BeNull();
    }

    [Fact]
    public void UpdateStudentProfile_ShouldChangeGroupId()
    {
        // Arrange
        var user = User.CreateStudent(
            Guid.NewGuid(),
            "Иванов",
            "Иван",
            "Иванович",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-22)),
            101
        );

        // Act
        user.UpdateStudentProfile(202);

        // Assert
        user.StudentProfile!.GroupId.Should().Be(202);
    }

    [Fact]
    public void UpdateStudentProfile_ShouldThrow_IfNoStudentProfile()
    {
        // Arrange
        var user = User.CreatePublisher(
            Guid.NewGuid(),
            "Иванов",
            "Иван",
            null,
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-22)),
            1
        );

        // Act
        Action act = () => user.UpdateStudentProfile(100);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*нет студенческого профиля*");
    }

    [Fact]
    public void UpdatePublisherProfile_ShouldThrow_IfNoPublisherProfile()
    {
        // Arrange
        var user = User.CreateStudent(
            Guid.NewGuid(),
            "Иванов",
            "Иван",
            null,
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-22)),
            101
        );

        // Act
        Action act = () => user.UpdatePublisherProfile(7);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*нет профиля публикатора*");
    }

    [Fact]
    public void CalculateAge_ShouldReturnCorrectAge()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30).AddDays(-1));

        // Act
        var age = User.CalculateAge(birthDate);

        // Assert
        age.Should().Be(30);
    }

    [Fact]
    public void CalculateAge_ShouldDecrement_WhenBirthdayNotPassedYet()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30).AddDays(+1));

        // Act
        var age = User.CalculateAge(birthDate);

        // Assert
        age.Should().Be(29);
    }
}
