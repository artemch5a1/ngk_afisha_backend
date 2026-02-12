using FluentAssertions;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class PostTests
{
    [Fact]
    public void Create_ShouldReturnValidPost()
    {
        // Arrange
        string title = "Главный инженер";
        int departmentId = 1;

        // Act
        var post = Post.Create(title, departmentId);

        // Assert
        post.Should().NotBeNull();
        post.Title.Should().Be(title);
        post.DepartmentId.Should().Be(departmentId);
        post.PostId.Should().Be(0); // устанавливается только при Restore
    }

    [Fact]
    public void Create_ShouldThrow_WhenTitleEmpty()
    {
        // Arrange
        string title = "";
        int departmentId = 1;

        // Act
        Action act = () => Post.Create(title, departmentId);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void Create_ShouldThrow_WhenTitleTooLong()
    {
        // Arrange
        string title = new string('A', 201);
        int departmentId = 1;

        // Act
        Action act = () => Post.Create(title, departmentId);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может превышать*");
    }

    [Fact]
    public void Restore_ShouldReturnPostWithId()
    {
        // Arrange
        int postId = 10;
        string title = "Менеджер проекта";
        int departmentId = 2;

        // Act
        var post = Post.Restore(postId, title, departmentId);

        // Assert
        post.PostId.Should().Be(postId);
        post.Title.Should().Be(title);
        post.DepartmentId.Should().Be(departmentId);
    }

    [Fact]
    public void UpdatePost_ShouldChangeValues()
    {
        // Arrange
        var post = Post.Create("Разработчик", 1);

        // Act
        post.UpdatePost("Старший разработчик", 2);

        // Assert
        post.Title.Should().Be("Старший разработчик");
        post.DepartmentId.Should().Be(2);
    }

    [Fact]
    public void UpdatePost_ShouldThrow_WhenTitleEmpty()
    {
        // Arrange
        var post = Post.Create("Тестировщик", 1);

        // Act
        Action act = () => post.UpdatePost("", 2);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void UpdatePost_ShouldThrow_WhenTitleTooLong()
    {
        // Arrange
        var post = Post.Create("Аналитик", 1);
        string longTitle = new string('B', 250);

        // Act
        Action act = () => post.UpdatePost(longTitle, 2);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может превышать*");
    }

    [Fact]
    public void AddDepartmentNavigation_ShouldAssignDepartment()
    {
        // Arrange
        var post = Post.Create("Менеджер", 1);
        var department = Department.CreateDepartment("Отдел IT");

        // Act
        post.AddDepartmentNavigation(department);

        // Assert
        post.Department.Should().Be(department);
    }
}
