using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;

namespace EventService.UnitTests.Models;

public class EventTests
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
        return Event.Create(
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
    }

    [Fact]
    public void Create_Should_Create_Valid_Event()
    {
        var ev = CreateValidEvent();

        Assert.Equal(ValidTitle, ev.Title);
        Assert.Equal(ValidShortDescription, ev.ShortDescription);
        Assert.Equal(ValidDescription, ev.Description);
        Assert.Equal(ValidFutureDate, ev.DateStart);
        Assert.Equal(ValidMinAge, ev.MinAge);
        Assert.Equal(ValidAuthor, ev.Author);
        Assert.Equal(ValidPreviewUrl, ev.PreviewUrl);
        Assert.Equal(ValidLocationId, ev.LocationId);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Too_Short()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                "Short",
                ValidShortDescription,
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge,
                ValidAuthor,
                ValidPreviewUrl
            )
        );
        Assert.Contains("Название события", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Too_Long()
    {
        var title = new string('A', 90);
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                title,
                ValidShortDescription,
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge,
                ValidAuthor,
                ValidPreviewUrl
            )
        );
        Assert.Contains("Название события", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_ShortDescription_Too_Short()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                ValidTitle,
                "Слишком коротк",
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge,
                ValidAuthor,
                ValidPreviewUrl
            )
        );
    }

    [Fact]
    public void Create_Should_Throw_When_Description_Too_Short()
    {
        var desc = new string('X', 10);
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                ValidTitle,
                ValidShortDescription,
                desc,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge,
                ValidAuthor,
                ValidPreviewUrl
            )
        );
        Assert.Contains("Описание события", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Invalid_MinAge()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                ValidTitle,
                ValidShortDescription,
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                21,
                ValidAuthor,
                ValidPreviewUrl
            )
        );
        Assert.Contains("Некорректное ограничение возраста", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Date_Is_In_Past()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                ValidTitle,
                ValidShortDescription,
                ValidDescription,
                DateTime.Now.AddDays(-1),
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge,
                ValidAuthor,
                ValidPreviewUrl
            )
        );
        Assert.Contains("Дата события", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Author_Empty()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Event.Create(
                Guid.NewGuid(),
                ValidTitle,
                ValidShortDescription,
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge,
                Guid.Empty,
                ValidPreviewUrl
            )
        );
        Assert.Contains("Некорректный автор", ex.Message);
    }

    [Fact]
    public void Restore_Should_Create_Event_Without_Validation()
    {
        var id = Guid.NewGuid();
        var ev = Event.Restore(
            id,
            "X",
            "Y",
            "Z",
            DateTime.MinValue,
            5,
            ValidGenreId,
            ValidTypeId,
            99,
            Guid.Empty,
            "url"
        );

        Assert.Equal(id, ev.EventId);
        Assert.Equal("X", ev.Title);
        Assert.Equal(5, ev.LocationId);
    }

    [Fact]
    public void Update_Should_Change_Fields_When_Author_Is_Valid()
    {
        var ev = CreateValidEvent();

        var newTitle = "Фестиваль современного искусства";
        var newShort = "Лучшие арт-проекты и выставки со всей страны!";
        var newDesc = new string('A', 100);
        var newDate = DateTime.Now.AddDays(10);
        var newAge = 18;
        var newLocationId = 2;

        ev.Update(
            ValidAuthor,
            newTitle,
            newShort,
            newDesc,
            newDate,
            newLocationId,
            ValidGenreId,
            ValidTypeId,
            newAge
        );

        Assert.Equal(newTitle, ev.Title);
        Assert.Equal(newShort, ev.ShortDescription);
        Assert.Equal(newDesc, ev.Description);
        Assert.Equal(newDate, ev.DateStart);
        Assert.Equal(newAge, ev.MinAge);
        Assert.Equal(newLocationId, ev.LocationId);
    }

    [Fact]
    public void Update_Should_Throw_When_User_Is_Not_Author()
    {
        var ev = CreateValidEvent();
        var anotherUser = Guid.NewGuid();

        var ex = Assert.Throws<NotFoundException>(() =>
            ev.Update(
                anotherUser,
                ValidTitle,
                ValidShortDescription,
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge
            )
        );

        Assert.Contains("Событие", ex.Message);
    }

    [Fact]
    public void Update_Should_Throw_When_Invalid_Data()
    {
        var ev = CreateValidEvent();

        Assert.Throws<DomainException>(() =>
            ev.Update(
                ValidAuthor,
                "",
                ValidShortDescription,
                ValidDescription,
                ValidFutureDate,
                ValidLocationId,
                ValidGenreId,
                ValidTypeId,
                ValidMinAge
            )
        );
    }

    [Fact]
    public void AddLocationNavigation_Should_Assign_Location()
    {
        var ev = CreateValidEvent();
        var location = Location.Create("adadadadadad", "adadadadadad");

        ev.AddLocationNavigation(location);

        Assert.Equal(location, ev.Location);
    }
}
