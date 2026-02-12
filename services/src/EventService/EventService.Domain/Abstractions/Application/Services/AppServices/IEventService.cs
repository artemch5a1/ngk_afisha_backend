using EventService.Domain.Abstractions.Specification;
using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface IEventService
{
    Task<List<Event>> GetAllEvent(
        ISpecification<Event>? specification,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsEventsExist(CancellationToken cancellationToken = default);

    Task<List<Event>> GetAllEvent(
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Event>> GetAllEventByAuthorId(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    );

    Task<Event?> GetEventById(Guid eventId, CancellationToken cancellationToken = default);

    Task<Event> CreateEvent(
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge,
        Guid author,
        CancellationToken cancellationToken = default
    );

    Task<Invitation> CreateInvitation(
        Guid eventId,
        Guid currentUser,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine,
        CancellationToken cancellationToken = default
    );

    Task<bool> UpdateInvitation(
        Guid eventId,
        Guid currentUser,
        Guid invitationId,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteInvitation(
        Guid eventId,
        Guid invitationId,
        Guid currentUser,
        CancellationToken cancellationToken = default
    );

    Task<bool> TakeRequestOnInvitation(
        Guid eventId,
        Guid invitationId,
        Guid studentId,
        CancellationToken cancellationToken = default
    );

    Task<bool> CancelRequestOnInvitation(
        Guid eventId,
        Guid invitationId,
        Guid studentId,
        CancellationToken cancellationToken = default
    );

    Task<bool> AcceptRequestOnInvitation(
        Guid eventId,
        Guid invitationId,
        Guid studentId,
        Guid currentUser,
        CancellationToken cancellationToken = default
    );

    Task<bool> RejectMemberOnInvitation(
        Guid eventId,
        Guid invitationId,
        Guid studentId,
        Guid currentUser,
        CancellationToken cancellationToken = default
    );

    Task<Event> UpdateEvent(
        Guid currentUser,
        Guid eventId,
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteEvent(Guid eventId, CancellationToken cancellationToken = default);
}
