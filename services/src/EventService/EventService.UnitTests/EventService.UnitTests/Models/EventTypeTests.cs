using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.UnitTests.Models;

public class EventTypeTests
{
    private const string ValidTitle = "Спорт";
    
    [Fact]
    public void Create_Should_Create_Valid_EventType()
    {
        // Act
        var eventType = EventType.Create(ValidTitle);

        // Assert
        Assert.Equal(ValidTitle, eventType.Title);
        Assert.Equal(0, eventType.TypeId);
    }
    
    [Fact]
    public void Create_Should_Throw_When_Title_Is_Empty()
    {
        Assert.Throws<DomainException>(() =>
            EventType.Create("")
        );
    }
    
    [Fact]
    public void Create_Should_Throw_When_Title_Too_Short()
    {
        Assert.Throws<DomainException>(() =>
            EventType.Create("К")
        );
    }
    
    [Fact]
    public void Create_Should_Throw_When_Title_Too_Long()
    {
        var title = new string('A', 46);

        Assert.Throws<DomainException>(() =>
            EventType.Create(title)
        );
    }
        
    [Fact]
    public void Restore_Should_Create_EventType_Without_Validation()
    {
        // Arrange
        int id = 123;
        string title = "";

        // Act
        var eventType = EventType.Restore(id, title);

        // Assert
        Assert.Equal(id, eventType.TypeId);
        Assert.Equal(title, eventType.Title);
    }
        
    [Fact]
    public void UpdateEventType_Should_Update_Values_When_Valid()
    {
        // Arrange
        var eventType = EventType.Create(ValidTitle);
        string newTitle = "Новая title";

        // Act
        eventType.UpdateEventType(newTitle);

        // Assert
        Assert.Equal(newTitle, eventType.Title);
    }
        
    [Fact]
    public void UpdateEventType_Should_Throw_When_Title_Too_Short()
    {
        var eventType = EventType.Create(ValidTitle);

        Assert.Throws<DomainException>(() =>
            eventType.UpdateEventType("a"));
    }
        
    [Fact]
    public void UpdateEventType_Should_Throw_When_Title_Too_Long()
    {
        var eventType = EventType.Create(ValidTitle);
        var title = new string('A', 100);

        Assert.Throws<DomainException>(() => eventType.UpdateEventType(title));
    }
}