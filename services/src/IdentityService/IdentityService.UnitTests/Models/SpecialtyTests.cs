using FluentAssertions;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class SpecialtyTests
{
    [Fact]
    public void CreateSpecialty_ShouldCreateValidSpecialty()
    {
        // Arrange
        string title = "Программная инженерия";

        // Act
        var specialty = Specialty.CreateSpecialty(title);

        // Assert
        specialty.Should().NotBeNull();
        specialty.SpecialtyTitle.Should().Be(title);
        specialty.SpecialtyId.Should().Be(0); // id задаётся только при Restore
    }

    [Fact]
    public void CreateSpecialty_ShouldThrow_WhenTitleIsEmpty()
    {
        // Arrange
        string title = "";

        // Act
        Action act = () => Specialty.CreateSpecialty(title);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void Restore_ShouldReturnSpecialtyWithId()
    {
        // Arrange
        int id = 10;
        string title = "Информационные системы";

        // Act
        var specialty = Specialty.Restore(id, title);

        // Assert
        specialty.SpecialtyId.Should().Be(id);
        specialty.SpecialtyTitle.Should().Be(title);
    }

    [Fact]
    public void SpecialityUpdate_ShouldChangeTitle()
    {
        // Arrange
        var specialty = Specialty.CreateSpecialty("Физика");
        string newTitle = "Прикладная физика";

        // Act
        specialty.SpecialityUpdate(newTitle);

        // Assert
        specialty.SpecialtyTitle.Should().Be(newTitle);
    }

    [Fact]
    public void SpecialityUpdate_ShouldThrow_WhenTitleEmpty()
    {
        // Arrange
        var specialty = Specialty.CreateSpecialty("Математика");

        // Act
        Action act = () => specialty.SpecialityUpdate("");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }
}
