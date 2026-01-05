using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.UnitTests.Models;

public class EventRoleTests
    {
        [Fact]
        public void Create_ShouldCreateEventRole_WhenValidData()
        {
            // Arrange
            var title = "Организатор";
            var description = "Отвечает за проведение мероприятия и координацию участников.";

            // Act
            var role = EventRole.Create(title, description);

            // Assert
            Assert.Equal(title, role.Title);
            Assert.Equal(description, role.Description);
            Assert.Equal(0, role.EventRoleId);
        }

        [Fact]
        public void Restore_ShouldRestoreEventRole_WithGivenId()
        {
            // Arrange
            var id = 10;
            var title = "Волонтер";
            var description = "Помогает с организацией мероприятия и встречей гостей.";

            // Act
            var role = EventRole.Restore(id, title, description);

            // Assert
            Assert.Equal(id, role.EventRoleId);
            Assert.Equal(title, role.Title);
            Assert.Equal(description, role.Description);
        }

        [Theory]
        [InlineData("", "Описание корректное длиной более 15 символов")]
        [InlineData("A", "Описание корректное длиной более 15 символов")]
        [InlineData("ОченьДлинноеНазваниеКотороеПревышает35Символов", "Описание корректное длиной более 15 символов")]
        public void Create_ShouldThrowException_WhenInvalidTitle(string title, string description)
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => EventRole.Create(title, description));
            Assert.Contains("Название роли", ex.Message);
        }

        [Theory]
        [InlineData("Орг", "")]
        [InlineData("Орг", "Коротко")]
        [InlineData("Орг", "Очень длинное описание, которое значительно превышает допустимый лимит в семьдесят пять символов и не должно пройти валидацию.")]
        public void Create_ShouldThrowException_WhenInvalidDescription(string title, string description)
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => EventRole.Create(title, description));
            Assert.Contains("Описание роли", ex.Message);
        }

        [Fact]
        public void UpdateRole_ShouldChangeTitleAndDescription_WhenValid()
        {
            // Arrange
            var role = EventRole.Create("Редактор", "Редактирует тексты и материалы мероприятия.");
            var newTitle = "Модератор";
            var newDescription = "Отвечает за порядок и соблюдение правил на мероприятии.";

            // Act
            role.UpdateRole(newTitle, newDescription);

            // Assert
            Assert.Equal(newTitle, role.Title);
            Assert.Equal(newDescription, role.Description);
        }

        [Fact]
        public void UpdateRole_ShouldThrowException_WhenInvalidData()
        {
            // Arrange
            var role = EventRole.Create("Роль", "Достаточно длинное описание роли.");
            var invalidTitle = "А";
            var invalidDescription = "Коротко";

            // Act & Assert
            Assert.Throws<DomainException>(() => role.UpdateRole(invalidTitle, invalidDescription));
        }
    }