using FluentAssertions;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class GroupTests
{
    [Fact]
        public void CreateGroup_ShouldCreateValidGroup()
        {
            // Arrange
            int course = 3;
            int numberGroup = 12;
            int specialtyId = 5;

            // Act
            var group = Group.CreateGroup(course, numberGroup, specialtyId);

            // Assert
            group.Should().NotBeNull();
            group.Course.Should().Be(course);
            group.NumberGroup.Should().Be(numberGroup);
            group.SpecialtyId.Should().Be(specialtyId);
            group.GroupId.Should().Be(0); // устанавливается только при Restore
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void CreateGroup_ShouldThrow_WhenCourseIsOutOfRange(int invalidCourse)
        {
            // Arrange
            int numberGroup = 2;
            int specialtyId = 3;

            // Act
            Action act = () => Group.CreateGroup(invalidCourse, numberGroup, specialtyId);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("*Курс должен быть в промежутке от 1 до 4*");
        }

        [Fact]
        public void Restore_ShouldReturnGroupWithId()
        {
            // Arrange
            int id = 10;
            int course = 2;
            int numberGroup = 15;
            int specialtyId = 7;

            // Act
            var group = Group.Restore(id, course, numberGroup, specialtyId);

            // Assert
            group.GroupId.Should().Be(id);
            group.Course.Should().Be(course);
            group.NumberGroup.Should().Be(numberGroup);
            group.SpecialtyId.Should().Be(specialtyId);
        }

        [Fact]
        public void UpdateGroup_ShouldChangeValues()
        {
            // Arrange
            var group = Group.CreateGroup(1, 5, 3);

            // Act
            group.UpdateGroup(2, 6, 4);

            // Assert
            group.Course.Should().Be(2);
            group.NumberGroup.Should().Be(6);
            group.SpecialtyId.Should().Be(4);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void UpdateGroup_ShouldThrow_WhenCourseInvalid(int invalidCourse)
        {
            // Arrange
            var group = Group.CreateGroup(1, 1, 1);

            // Act
            Action act = () => group.UpdateGroup(invalidCourse, 10, 2);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("*Курс должен быть в промежутке от 1 до 4*");
        }

        [Fact]
        public void AddSpecialtyNavigation_ShouldAssignSpecialty()
        {
            // Arrange
            var group = Group.CreateGroup(2, 10, 3);
            var specialty = Specialty.Restore(1, "Информатика");

            // Act
            group.AddSpecialtyNavigation(specialty);

            // Assert
            group.Specialty.Should().Be(specialty);
        }

        [Fact]
        public void LetterSpecialty_ShouldReturnFirstLetterOfSpecialtyTitle()
        {
            // Arrange
            var group = Group.CreateGroup(2, 1, 5);
            var specialty = Specialty.Restore(1, "Физика");
            group.AddSpecialtyNavigation(specialty);

            // Act
            var letter = group.LetterSpecialty;

            // Assert
            letter.Should().Be('Ф');
        }

        [Fact]
        public void LetterSpecialty_ShouldReturnSpace_WhenNoSpecialty()
        {
            // Arrange
            var group = Group.CreateGroup(1, 1, 1);

            // Act
            var letter = group.LetterSpecialty;

            // Assert
            letter.Should().Be(' ');
        }

        [Fact]
        public void GetIdentityGroup_ShouldReturnFormattedName()
        {
            // Arrange
            var group = Group.CreateGroup(3, 15, 1);
            var specialty = Specialty.Restore(2, "Математика");
            group.AddSpecialtyNavigation(specialty);

            // Act
            var result = group.GetIdentityGroup();

            // Assert
            result.Should().Be("315М");
        }

        [Fact]
        public void GetIdentityGroup_ShouldIncludeSpace_WhenNoSpecialty()
        {
            // Arrange
            var group = Group.CreateGroup(2, 8, 1);

            // Act
            var result = group.GetIdentityGroup();

            // Assert
            result.Should().Be("28 ");
        }
}