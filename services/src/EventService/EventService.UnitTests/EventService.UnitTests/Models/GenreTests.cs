using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.UnitTests.Models;

public class GenreTests
{
    private const string ValidTitle = "Комедия";

    [Fact]
    public void Create_Should_Create_Valid_Genre()
    {
        // Act
        var genre = Genre.Create(ValidTitle);

        // Assert
        Assert.Equal(ValidTitle, genre.Title);
        Assert.Equal(0, genre.GenreId);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Is_Empty()
    {
        Assert.Throws<DomainException>(() => Genre.Create(""));
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Too_Short()
    {
        Assert.Throws<DomainException>(() => Genre.Create("К"));
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Too_Long()
    {
        var title = new string('A', 46);

        Assert.Throws<DomainException>(() => Genre.Create(title));
    }

    [Fact]
    public void Restore_Should_Create_Genre_Without_Validation()
    {
        // Arrange
        int id = 123;
        string title = "Т";

        // Act
        var genre = Genre.Restore(id, title);

        // Assert
        Assert.Equal(id, genre.GenreId);
        Assert.Equal(title, genre.Title);
    }

    [Fact]
    public void UpdateGenre_Should_Update_Values_When_Valid()
    {
        // Arrange
        var genre = Genre.Create(ValidTitle);
        string newTitle = "Новая title";

        // Act
        genre.UpdateGenre(newTitle);

        // Assert
        Assert.Equal(newTitle, genre.Title);
    }

    [Fact]
    public void UpdateGenre_Should_Throw_When_Title_Too_Short()
    {
        var genre = Genre.Create(ValidTitle);

        Assert.Throws<DomainException>(() => genre.UpdateGenre("a"));
    }

    [Fact]
    public void UpdateGenre_Should_Throw_When_Title_Too_Long()
    {
        var genre = Genre.Create(ValidTitle);
        var title = new string('A', 100);

        Assert.Throws<DomainException>(() => genre.UpdateGenre(title));
    }
}
