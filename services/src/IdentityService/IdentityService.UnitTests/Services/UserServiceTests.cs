using FluentAssertions;
using IdentityService.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using Moq;

namespace IdentityService.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _service = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnList()
        {
            // Arrange
            var users = new List<User>
            {
                User.CreateStudent(Guid.NewGuid(), "Иванов", "Иван", null, new DateOnly(2000, 1, 1), 101),
                User.CreatePublisher(Guid.NewGuid(), "Петров", "Пётр", "Петрович", new DateOnly(1995, 5, 5), 5)
            };

            _userRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllUsers();

            // Assert
            result.Should().HaveCount(2);
            result[0].StudentProfile.Should().NotBeNull();
            result[1].PublisherProfile.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenExists()
        {
            var user = User.CreateStudent(Guid.NewGuid(), "Иванов", "Иван", null, new DateOnly(2001, 1, 1), 10);

            _userRepoMock.Setup(r => r.GetById(user.UserId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(user);

            var result = await _service.GetUserById(user.UserId);

            result.Should().NotBeNull();
            result!.Surname.Should().Be("Иванов");
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenNotFound()
        {
            _userRepoMock.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User?)null);

            var result = await _service.GetUserById(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateStudent_ShouldCreateAndSaveUser()
        {
            var userId = Guid.NewGuid();

            _userRepoMock.Setup(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User u, CancellationToken _) => u);

            var result = await _service.CreateStudent(userId, "Иванов", "Иван", null, new DateOnly(2000, 1, 1), 101);

            result.StudentProfile.Should().NotBeNull();
            result.Surname.Should().Be("Иванов");
            _userRepoMock.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePublisher_ShouldCreateAndSaveUser()
        {
            var userId = Guid.NewGuid();

            _userRepoMock.Setup(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User u, CancellationToken _) => u);

            var result = await _service.CreatePublisher(userId, "Петров", "Пётр", "Петрович", new DateOnly(1990, 5, 5), 1);

            result.PublisherProfile.Should().NotBeNull();
            result.Name.Should().Be("Пётр");
            _userRepoMock.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateStudent_ShouldThrow_WhenTooYoung()
        {
            var userId = Guid.NewGuid();
            var tooYoung = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10));

            Func<Task> act = async () => await _service.CreateStudent(userId, "Иванов", "Иван", null, tooYoung, 1);

            await act.Should().ThrowAsync<DomainException>()
                     .WithMessage("*младше 14 лет*");
        }

        [Fact]
        public async Task UpdateUserInfo_ShouldUpdateFields_WhenUserExists()
        {
            var user = User.CreateStudent(Guid.NewGuid(), "Иванов", "Иван", null, new DateOnly(2000, 1, 1), 5);
            _userRepoMock.Setup(r => r.FindAsync(user.UserId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(user);
            _userRepoMock.Setup(r => r.Update(user, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _service.UpdateUserInfo(user.UserId, "Сидоров", "Сидор", null, new DateOnly(1999, 1, 1));

            result.Should().BeTrue();
            user.Surname.Should().Be("Сидоров");
            user.Name.Should().Be("Сидор");
        }

        [Fact]
        public async Task UpdateUserInfo_ShouldThrow_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User?)null);

            Func<Task> act = async () => await _service.UpdateUserInfo(Guid.NewGuid(), "А", "Б", null, new DateOnly(2000, 1, 1));

            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("*Пользователь*");
        }

        [Fact]
        public async Task UpdateStudentProfile_ShouldUpdate_WhenExists()
        {
            var user = User.CreateStudent(Guid.NewGuid(), "Иванов", "Иван", null, new DateOnly(2000, 1, 1), 5);

            _userRepoMock.Setup(r => r.GetById(user.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateStudentProfile(user, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _service.UpdateStudentProfile(user.UserId, 777);

            result.Should().BeTrue();
            user.StudentProfile!.GroupId.Should().Be(777);
        }

        [Fact]
        public async Task UpdateStudentProfile_ShouldThrow_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User?)null);

            Func<Task> act = async () => await _service.UpdateStudentProfile(Guid.NewGuid(), 1);

            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("*Пользователь*");
        }

        [Fact]
        public async Task UpdatePublisherProfile_ShouldUpdate_WhenExists()
        {
            var user = User.CreatePublisher(Guid.NewGuid(), "Петров", "Пётр", null, new DateOnly(1999, 1, 1), 5);

            _userRepoMock.Setup(r => r.GetById(user.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdatePublisherProfile(user, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _service.UpdatePublisherProfile(user.UserId, 11);

            result.Should().BeTrue();
            user.PublisherProfile!.PostId.Should().Be(11);
        }

        [Fact]
        public async Task UpdatePublisherProfile_ShouldThrow_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User?)null);

            Func<Task> act = async () => await _service.UpdatePublisherProfile(Guid.NewGuid(), 1);

            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("*Пользователь*");
        }
}