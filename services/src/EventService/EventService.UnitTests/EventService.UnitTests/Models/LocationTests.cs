using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.UnitTests.Models;

public class LocationTests
{
    private const string ValidTitle = "Главный концертный зал";
    private const string ValidAddress = "г. Москва, ул. Арбат, 15";

    [Fact]
    public void Create_Should_Create_Valid_Location()
    {
        // Act
        var location = Location.Create(ValidTitle, ValidAddress);

        // Assert
        Assert.Equal(ValidTitle, location.Title);
        Assert.Equal(ValidAddress, location.Address);
        Assert.Equal(0, location.LocationId); // т.к. создаётся новый
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Is_Empty()
    {
        var ex = Assert.Throws<DomainException>(() => Location.Create("", ValidAddress));
        Assert.Contains("Название локации", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Too_Short()
    {
        var ex = Assert.Throws<DomainException>(() => Location.Create("Каф", ValidAddress));
        Assert.Contains("Название локации", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Too_Long()
    {
        var title = new string('A', 46);

        var ex = Assert.Throws<DomainException>(() => Location.Create(title, ValidAddress));
        Assert.Contains("Название локации", ex.Message);
    }

    [Fact]
    public void Restore_Should_Create_Location_Without_Validation()
    {
        // Arrange
        int id = 123;
        string title = "Тест";
        string address = "г. Тула";

        // Act
        var location = Location.Restore(id, title, address);

        // Assert
        Assert.Equal(id, location.LocationId);
        Assert.Equal(title, location.Title);
        Assert.Equal(address, location.Address);
    }

    [Fact]
    public void UpdateLocation_Should_Update_Values_When_Valid()
    {
        // Arrange
        var location = Location.Create(ValidTitle, ValidAddress);
        string newTitle = "Новая сцена";
        string newAddress = "г. Санкт-Петербург, Невский проспект, 10";

        // Act
        location.UpdateLocation(newTitle, newAddress);

        // Assert
        Assert.Equal(newTitle, location.Title);
        Assert.Equal(newAddress, location.Address);
    }

    [Fact]
    public void UpdateLocation_Should_Throw_When_Title_Too_Short()
    {
        var location = Location.Create(ValidTitle, ValidAddress);

        var ex = Assert.Throws<DomainException>(() => location.UpdateLocation("abc", ValidAddress));

        Assert.Contains("Название локации", ex.Message);
    }

    [Fact]
    public void UpdateLocation_Should_Throw_When_Title_Too_Long()
    {
        var location = Location.Create(ValidTitle, ValidAddress);
        var title = new string('A', 100);

        var ex = Assert.Throws<DomainException>(() => location.UpdateLocation(title, ValidAddress));

        Assert.Contains("Название локации", ex.Message);
    }
}
