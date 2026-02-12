using FluentAssertions;
using IdentityService.Domain.Abstractions.Infrastructure.Utils;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.AccountContext;
using Moq;

namespace IdentityService.UnitTests.Models;

public class AccountTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    public AccountTests()
    {
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _passwordHasherMock
            .Setup(p => p.HashPassword(It.IsAny<string>()))
            .Returns((string s) => $"HASHED_{s}");
        _passwordHasherMock
            .Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string password, string hash) => hash == $"HASHED_{password}");
    }

    [Fact]
    public void CreateStudentAccount_ShouldCreateAccountWithRoleUser()
    {
        // Arrange
        string email = "user@test.com";
        string password = "1234";

        // Act
        var account = Account.CreateStudentAccount(email, password, _passwordHasherMock.Object);

        // Assert
        account.Should().NotBeNull();
        account.Email.Should().Be(email.ToLowerInvariant());
        account.PasswordHash.Should().Be($"HASHED_{password}");
        account.AccountRole.Should().Be(Role.User);
    }

    [Fact]
    public void CreatePublisherAccount_ShouldCreateAccountWithRolePublisher()
    {
        // Arrange
        string email = "publisher@test.com";
        string password = "1234";

        // Act
        var account = Account.CreatePublisherAccount(email, password, _passwordHasherMock.Object);

        // Assert
        account.AccountRole.Should().Be(Role.Publisher);
        account.Email.Should().Be(email.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateAccount_ShouldThrow_WhenEmailEmpty(string? email)
    {
        // Act
        Action act = () => Account.CreateStudentAccount(email!, "1234", _passwordHasherMock.Object);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не может быть пустым*");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("d..@domain.com")]
    [InlineData("domain@domain..com")]
    [InlineData("domain@domain.com..")]
    public void CreateAccount_ShouldThrow_WhenEmailInvalid(string email)
    {
        // Act
        Action act = () => Account.CreateStudentAccount(email, "1234", _passwordHasherMock.Object);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Некорректный email*");
    }

    [Fact]
    public void CreateAccount_ShouldThrow_WhenPasswordTooShort()
    {
        // Act
        Action act = () =>
            Account.CreateStudentAccount("user@test.com", "123", _passwordHasherMock.Object);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не меньше 4*");
    }

    [Fact]
    public void Restore_ShouldThrow_WhenAccountIdEmpty()
    {
        // Act
        Action act = () =>
            Account.Restore(Guid.Empty, "user@test.com", "HASHED_1234", DateTime.UtcNow, Role.User);

        // Assert
        act.Should().Throw<InvalidDataException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void Restore_ShouldThrow_WhenEmailEmpty()
    {
        // Act
        Action act = () =>
            Account.Restore(Guid.NewGuid(), "", "HASHED_1234", DateTime.UtcNow, Role.User);

        // Assert
        act.Should().Throw<InvalidDataException>().WithMessage("*не может быть пустым*");
    }

    [Fact]
    public void ChangePassword_ShouldUpdatePassword_WhenOldPasswordCorrect()
    {
        // Arrange
        var account = Account.CreateStudentAccount(
            "user@test.com",
            "1234",
            _passwordHasherMock.Object
        );

        // Act
        account.ChangePassword("1234", "5678", _passwordHasherMock.Object);

        // Assert
        account.PasswordHash.Should().Be("HASHED_5678");
    }

    [Fact]
    public void ChangePassword_ShouldThrow_WhenOldPasswordIncorrect()
    {
        // Arrange
        var account = Account.CreateStudentAccount(
            "user@test.com",
            "1234",
            _passwordHasherMock.Object
        );

        // Act
        Action act = () => account.ChangePassword("wrong", "5678", _passwordHasherMock.Object);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Неверный пароль*");
    }

    [Fact]
    public void ChangePassword_ShouldThrow_WhenNewPasswordTooShort()
    {
        // Arrange
        var account = Account.CreateStudentAccount(
            "user@test.com",
            "1234",
            _passwordHasherMock.Object
        );

        // Act
        Action act = () => account.ChangePassword("1234", "123", _passwordHasherMock.Object);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*не меньше 4*");
    }

    [Theory]
    [InlineData("user@test.com", true)]
    [InlineData("user@domain.com", true)]
    [InlineData("user@", false)]
    [InlineData("", false)]
    public void IsValidEmail_ShouldReturnCorrectResult(string email, bool expected)
    {
        // Act
        var result = Account.IsValidEmail(email);

        // Assert
        result.Should().Be(expected);
    }
}
