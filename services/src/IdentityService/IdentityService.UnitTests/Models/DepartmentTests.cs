using FluentAssertions;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class DepartmentTests
{
    [Fact]
    public void CreateDepartment_ShouldCreateValidDepartment()
    {
        // Arrange
        string title = "Отдел разработки";

        // Act
        var department = Department.CreateDepartment(title);

        // Assert
        department.Should().NotBeNull();
        department.Title.Should().Be(title);
        department.DepartmentId.Should().Be(0); // устанавливается только при Restore
    }

    [Fact]
    public void CreateDepartment_ShouldThrow_WhenTitleIsEmpty()
    {
        // Arrange
        string title = "";

        // Act
        Action act = () => Department.CreateDepartment(title);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void CreateDepartment_ShouldThrow_WhenTitleTooLong()
    {
        // Arrange
        string title = new string('A', 201);

        // Act
        Action act = () => Department.CreateDepartment(title);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может превышать*");
    }

    [Fact]
    public void Restore_ShouldReturnDepartmentWithId()
    {
        // Arrange
        int id = 10;
        string title = "Отдел тестирования";

        // Act
        var department = Department.Restore(id, title);

        // Assert
        department.DepartmentId.Should().Be(id);
        department.Title.Should().Be(title);
    }

    [Fact]
    public void UpdateDepartment_ShouldChangeTitle()
    {
        // Arrange
        var department = Department.CreateDepartment("Отдел продаж");
        string newTitle = "Отдел маркетинга";

        // Act
        department.UpdateDepartment(newTitle);

        // Assert
        department.Title.Should().Be(newTitle);
    }

    [Fact]
    public void UpdateDepartment_ShouldThrow_WhenTitleIsEmpty()
    {
        // Arrange
        var department = Department.CreateDepartment("Отдел исследований");

        // Act
        Action act = () => department.UpdateDepartment("");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void UpdateDepartment_ShouldThrow_WhenTitleTooLong()
    {
        // Arrange
        var department = Department.CreateDepartment("Отдел закупок");
        string longTitle = new string('B', 250);

        // Act
        Action act = () => department.UpdateDepartment(longTitle);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может превышать*");
    }
}
