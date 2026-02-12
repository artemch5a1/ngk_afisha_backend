using EventService.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Models;
using Moq;

namespace EventService.UnitTests.Services
{
    public class EventServiceCoreTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly EventServiceCore _service;

        public EventServiceCoreTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _service = new EventServiceCore(_eventRepositoryMock.Object);
        }

        private static Event CreateValidEvent(Guid? authorId = null)
        {
            return Event.Create(
                Guid.NewGuid(),
                "Valid Title",
                "Valid short description",
                "This is a valid long description with more than 35 characters.",
                DateTime.UtcNow.AddDays(2),
                1,
                1,
                1,
                18,
                authorId ?? Guid.NewGuid(),
                "preview.png"
            );
        }

        private static Invitation CreateValidInvitation(Event ev, Guid author)
        {
            return ev.AddNewInvitation(
                author,
                1,
                "Valid short description for invitation",
                "Valid invitation description longer than 35 chars.",
                3,
                DateTime.UtcNow.AddDays(1)
            );
        }

        [Fact]
        public async Task GetAllEvent_ShouldReturnEvents()
        {
            var events = new List<Event> { CreateValidEvent() };
            _eventRepositoryMock
                .Setup(r => r.GetAll(It.IsAny<PaginationContract>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);

            var result = await _service.GetAllEvent();

            Assert.Equal(events, result);
        }

        [Fact]
        public async Task GetEventById_ShouldReturnEvent()
        {
            var ev = CreateValidEvent();
            _eventRepositoryMock
                .Setup(r => r.GetById(ev.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ev);

            var result = await _service.GetEventById(ev.EventId);

            Assert.Equal(ev, result);
        }

        // ========== 🔹 Create Event ==========

        [Fact]
        public async Task CreateEvent_ShouldReturnCreatedEvent_WhenDataValid()
        {
            var ev = CreateValidEvent();
            _eventRepositoryMock
                .Setup(r => r.Create(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ev);

            var result = await _service.CreateEvent(
                ev.Title,
                ev.ShortDescription,
                ev.Description,
                ev.DateStart,
                ev.LocationId,
                ev.GenreId,
                ev.TypeId,
                ev.MinAge,
                ev.Author
            );

            Assert.NotNull(result);
            Assert.Equal(ev.Title, result.Title);
            _eventRepositoryMock.Verify(
                r => r.Create(It.IsAny<Event>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateEvent_ShouldThrow_WhenInvalidData()
        {
            await Assert.ThrowsAsync<DomainException>(() =>
                _service.CreateEvent(
                    "Bad", // too short
                    "short desc", // too short
                    "desc", // too short
                    DateTime.UtcNow.AddDays(-1), // past date
                    1,
                    1,
                    1,
                    99, // invalid age
                    Guid.NewGuid()
                )
            );
        }

        [Fact]
        public async Task CreateInvitation_ShouldThrow_WhenEventNotFound()
        {
            _eventRepositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.CreateInvitation(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    1,
                    "short",
                    "desc",
                    2,
                    DateTime.UtcNow
                )
            );
        }

        [Fact]
        public async Task CreateInvitation_ShouldReturnInvitation_WhenValid()
        {
            var ev = CreateValidEvent();
            _eventRepositoryMock
                .Setup(r => r.GetById(ev.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ev);
            _eventRepositoryMock
                .Setup(r => r.UpdateEventAggregate(ev, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _service.CreateInvitation(
                ev.EventId,
                ev.Author,
                1,
                "Short invitation desc valid",
                "Long valid description more than 35 chars",
                3,
                DateTime.UtcNow.AddDays(1)
            );

            Assert.NotNull(result);
            Assert.Equal(ev.EventId, result.EventId);
            _eventRepositoryMock.Verify(
                r => r.UpdateEventAggregate(It.IsAny<Event>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateInvitation_ShouldThrow_WhenUpdateFails()
        {
            var ev = CreateValidEvent();
            _eventRepositoryMock
                .Setup(r => r.GetById(ev.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ev);
            _eventRepositoryMock
                .Setup(r =>
                    r.UpdateEventAggregate(It.IsAny<Event>(), It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<DomainException>(() =>
                _service.CreateInvitation(
                    ev.EventId,
                    ev.Author,
                    1,
                    "Valid short",
                    "Valid desc more than 35 chars",
                    2,
                    DateTime.UtcNow.AddDays(1)
                )
            );
        }

        [Fact]
        public async Task UpdateEvent_ShouldThrow_WhenEventNotFound()
        {
            _eventRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpdateEvent(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "title",
                    "short",
                    "desc",
                    DateTime.UtcNow,
                    1,
                    1,
                    1,
                    18
                )
            );
        }

        [Fact]
        public async Task UpdateEvent_ShouldUpdate_WhenValid()
        {
            var ev = CreateValidEvent();
            _eventRepositoryMock
                .Setup(r => r.FindAsync(ev.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ev);
            _eventRepositoryMock
                .Setup(r => r.Update(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _service.UpdateEvent(
                ev.Author,
                ev.EventId,
                "Updated title",
                "Updated short description",
                "Updated valid long description exceeding 35 chars",
                DateTime.UtcNow.AddDays(5),
                2,
                2,
                2,
                16
            );

            _eventRepositoryMock.Verify(
                r => r.Update(It.IsAny<Event>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteEvent_ShouldReturnTrue_WhenRepositorySucceeds()
        {
            _eventRepositoryMock
                .Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _service.DeleteEvent(Guid.NewGuid());

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteInvitation_ShouldThrow_WhenEventNotFound()
        {
            _eventRepositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteInvitation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
            );
        }
    }
}
