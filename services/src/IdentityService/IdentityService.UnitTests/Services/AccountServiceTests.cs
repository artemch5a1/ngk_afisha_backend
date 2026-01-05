using FluentAssertions;
using IdentityService.Application.Services.AccountContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.AccountContext;
using IdentityService.Domain.Abstractions.Infrastructure.Utils;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.AccountContext;
using Moq;

namespace IdentityService.UnitTests.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _accountRepoMock = new Mock<IAccountRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _passwordHasherMock
                .Setup(h => h.HashPassword(It.IsAny<string>()))
                .Returns<string>(p => $"HASHED_{p}");

            _passwordHasherMock
                .Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((password, hash) => hash == $"HASHED_{password}");

            _service = new AccountService(_accountRepoMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnAccount_WhenPasswordCorrect()
        {
            // Arrange
            var account = Account.CreateStudentAccount("user@test.com", "1234", _passwordHasherMock.Object);
            _accountRepoMock.Setup(r => r.FindOnlyUsersByEmail("user@test.com", It.IsAny<CancellationToken>()))
                            .ReturnsAsync(account);

            // Act
            var result = await _service.LoginAsync("user@test.com", "1234");

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("user@test.com");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIncorrect()
        {
            // Arrange
            var account = Account.CreateStudentAccount("user@test.com", "1234", _passwordHasherMock.Object);
            _accountRepoMock.Setup(r => r.FindOnlyUsersByEmail("user@test.com", It.IsAny<CancellationToken>()))
                            .ReturnsAsync(account);

            // Act
            var result = await _service.LoginAsync("user@test.com", "wrong");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenAccountNotFound()
        {
            // Arrange
            _accountRepoMock.Setup(r => r.FindOnlyUsersByEmail("missing@test.com", It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Account?)null);

            // Act
            var result = await _service.LoginAsync("missing@test.com", "1234");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAdminAsync_ShouldReturnAdmin_WhenPasswordCorrect()
        {
            // Arrange
            var admin = Account.CreatePublisherAccount("admin@test.com", "adminpass", _passwordHasherMock.Object);
            _accountRepoMock.Setup(r => r.FindAdminByEmail("admin@test.com", It.IsAny<CancellationToken>()))
                            .ReturnsAsync(admin);

            // Act
            var result = await _service.LoginAdminAsync("admin@test.com", "adminpass");

            // Assert
            result.Should().NotBeNull();
            result!.AccountRole.Should().Be(Role.Publisher);
        }

        [Fact]
        public async Task CreateStudentAccount_ShouldCreateAndSaveAccount()
        {
            // Arrange
            _accountRepoMock.Setup(r => r.Create(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Account a, CancellationToken _) => a);

            // Act
            var result = await _service.CreateStudentAccount("user@test.com", "1234");

            // Assert
            result.Email.Should().Be("user@test.com");
            result.AccountRole.Should().Be(Role.User);
            _accountRepoMock.Verify(r => r.Create(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePublisherAccount_ShouldCreateAndSaveAccount()
        {
            // Arrange
            _accountRepoMock.Setup(r => r.Create(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Account a, CancellationToken _) => a);

            // Act
            var result = await _service.CreatePublisherAccount("publisher@test.com", "abcd");

            // Assert
            result.AccountRole.Should().Be(Role.Publisher);
            result.Email.Should().Be("publisher@test.com");
            _accountRepoMock.Verify(r => r.Create(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateStudentAccount_ShouldThrow_WhenEmailInvalid()
        {
            // Act
            Func<Task> act = async () => await _service.CreateStudentAccount("invalid-email", "1234");

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                     .WithMessage("*Некорректный email*");
        }
        
        [Fact]
        public async Task CreateStudentAccount_ShouldThrow_WhenEmailEmpty()
        {
            await Assert.ThrowsAsync<DomainException>(() =>
                _service.CreateStudentAccount("", "1234")
            );
        }

        
        [Fact]
        public async Task CreatePublisherAccount_ShouldThrow_WhenPasswordTooShort()
        {
            await Assert.ThrowsAsync<DomainException>(() =>
                _service.CreatePublisherAccount("publisher@test.com", "123")
            );
        }


        [Fact]
        public async Task ChangeAccountPassword_ShouldUpdate_WhenOldPasswordCorrect()
        {
            // Arrange
            var account = Account.CreateStudentAccount("user@test.com", "1234", _passwordHasherMock.Object);
            _accountRepoMock.Setup(r => r.FindAsync(account.AccountId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(account);
            _accountRepoMock.Setup(r => r.Update(account, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(true);

            // Act
            var result = await _service.ChangeAccountPassword(account.AccountId, "1234", "5678");

            // Assert
            result.Should().BeTrue();
            account.PasswordHash.Should().Be("HASHED_5678");
            _accountRepoMock.Verify(r => r.Update(account, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangeAccountPassword_ShouldThrow_WhenAccountNotFound()
        {
            // Arrange
            _accountRepoMock.Setup(r => r.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Account?)null);

            // Act
            Func<Task> act = async () => await _service.ChangeAccountPassword(Guid.NewGuid(), "1234", "5678");

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("*Аккаунт*");
        }

        [Fact]
        public async Task ChangeAccountPassword_ShouldThrow_WhenOldPasswordIncorrect()
        {
            // Arrange
            var account = Account.CreateStudentAccount("user@test.com", "1234", _passwordHasherMock.Object);
            _accountRepoMock.Setup(r => r.FindAsync(account.AccountId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(account);

            // Act
            Func<Task> act = async () => await _service.ChangeAccountPassword(account.AccountId, "wrong", "5678");

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                     .WithMessage("*Неверный пароль*");
        }

        [Fact]
        public async Task GetAccountById_ShouldReturnAccount_WhenExists()
        {
            // Arrange
            var account = Account.CreateStudentAccount("user@test.com", "1234", _passwordHasherMock.Object);
            _accountRepoMock.Setup(r => r.GetById(account.AccountId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(account);

            // Act
            var result = await _service.GetAccountById(account.AccountId);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("user@test.com");
        }

        [Fact]
        public async Task GetAllAccounts_ShouldReturnList()
        {
            // Arrange
            var list = new List<Account>
            {
                Account.CreateStudentAccount("a@test.com", "1111", _passwordHasherMock.Object),
                Account.CreatePublisherAccount("b@test.com", "2222", _passwordHasherMock.Object)
            };

            _accountRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(list);

            // Act
            var result = await _service.GetAllAccounts();

            // Assert
            result.Should().HaveCount(2);
            result[0].AccountRole.Should().Be(Role.User);
            result[1].AccountRole.Should().Be(Role.Publisher);
        }
}