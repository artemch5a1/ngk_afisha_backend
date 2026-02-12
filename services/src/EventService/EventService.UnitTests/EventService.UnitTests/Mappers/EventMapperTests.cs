using EventService.Domain.Abstractions.Infrastructure.Mapping;
using EventService.Domain.Models;
using EventService.Infrastructure.Entites;
using EventService.Infrastructure.Implementations.Mapping;

namespace EventService.UnitTests.Mappers;

public class EventMapperTests
{
    private static readonly Guid ValidAuthor = Guid.NewGuid();
    private const int ValidLocationId = 1;
    private const int ValidTypeId = 1;
    private const int ValidGenreId = 1;
    private const string ValidTitle = "Концерт группы Кино";
    private const string ValidShortDescription = "Лучшие хиты Виктора Цоя вживую на большой сцене!";
    private const string ValidDescription =
        "Грандиозный вечер с живым исполнением легендарных хитов группы 'Кино'. Звуковое шоу и атмосфера 80-х ждут вас!";
    private static readonly DateTime ValidFutureDate = DateTime.Now.AddDays(5);
    private const int ValidMinAge = 16;
    private const string ValidPreviewUrl = "https://cdn.example.com/poster.jpg";

    private Event CreateValidEvent()
    {
        Event @event = Event.Restore(
            Guid.NewGuid(),
            ValidTitle,
            ValidShortDescription,
            ValidDescription,
            ValidFutureDate,
            ValidLocationId,
            ValidGenreId,
            ValidTypeId,
            ValidMinAge,
            ValidAuthor,
            ValidPreviewUrl
        );

        Location location = Location.Restore(1, "Testing", "TestingAddress");

        EventType eventType = EventType.Restore(1, "Testing");

        Genre genre = Genre.Restore(1, "Testing");

        @event.AddLocationNavigation(location);

        @event.AddEventTypeNavigation(eventType);

        @event.AddGenreNavigation(genre);

        return @event;
    }

    private EventEntity TakeEntity()
    {
        Event @event = CreateValidEvent();

        EventEntity entity = EventEntity.ToEntity(@event);

        entity.Location = LocationEntity.ToEntity(@event.Location);

        entity.Genre = GenreEntity.ToEntity(@event.Genre);

        entity.Type = EventTypeEntity.ToEntity(@event.Type);

        return entity;
    }

    [Fact]
    public void ToDomain_ShouldReturnDomainWithLocation()
    {
        //Arrange
        EventEntity eventEntity = TakeEntity();

        IEntityMapper<EventEntity, Event> entityMapper = new EventMapper();

        //Act

        Event returnsEvent = entityMapper.ToDomain(eventEntity);

        //Assert
        Assert.Equal(returnsEvent.EventId, eventEntity.EventId);
        Assert.NotNull(returnsEvent.Location);
        Assert.NotNull(returnsEvent.Type);
        Assert.NotNull(returnsEvent.Genre);

        Assert.Equal(returnsEvent.Location.LocationId, eventEntity.Location.LocationId);
        Assert.Equal(returnsEvent.Genre.GenreId, eventEntity.Genre.GenreId);
        Assert.Equal(returnsEvent.Type.TypeId, eventEntity.Type.TypeId);
    }
}
