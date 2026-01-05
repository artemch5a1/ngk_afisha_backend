using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Abstractions.Specification;
using EventService.Domain.Contract;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Enums;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class EventServiceCore : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventServiceCore(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<bool> IsEventsExist(CancellationToken cancellationToken = default) 
        => await _eventRepository.EventsIsExist(cancellationToken);
    
    public async Task<List<Event>> GetAllEvent(ISpecification<Event>? specification, PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        return await _eventRepository.GetAll(specification, contract, cancellationToken);
    }
    
    public async Task<List<Event>> GetAllEvent(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        return await _eventRepository.GetAll(contract, cancellationToken);
    }

    public async Task<List<Event>> GetAllEventByAuthorId(Guid authorId, PaginationContract? contract = null,
        CancellationToken cancellationToken = default)
    {
        return await _eventRepository.GetAllEventByAuthorId(authorId, contract, cancellationToken);
    }

    public async Task<Event?> GetEventById(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _eventRepository.GetById(eventId, cancellationToken);
    }

    public async Task<Event> CreateEvent(
        string title, 
        string shortDescription, 
        string description, 
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge, 
        Guid author,
        CancellationToken cancellationToken = default)
    {
        Guid guid = Guid.NewGuid();
        
        string previewUrl = $"events/{guid}/preview/preview.jpg";
        
        Event eventModel = Event.Create(
            guid, 
            title, shortDescription, 
            description, 
            dateStart, 
            locationId,
            genreId, 
            typeId,
            minAge, 
            author,
            previewUrl);

        return await _eventRepository.Create(eventModel, cancellationToken);
    }


    public async Task<Invitation> CreateInvitation(
        Guid eventId, 
        Guid currentUser, 
        int roleId, 
        string shortDescription, 
        string description, 
        int requiredMember, 
        DateTime deadLine,
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);

        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        Invitation invitation = @event.AddNewInvitation(currentUser, roleId, shortDescription, description, requiredMember, deadLine);

        bool result = await _eventRepository.UpdateEventAggregate(@event, cancellationToken);

        if (!result)
            throw new DatabaseException("Ошибка добавления приглашения", ApiErrorType.InternalServerError);
        
        return invitation;
    }

    public async Task<bool> UpdateInvitation(
        Guid eventId, 
        Guid currentUser, 
        Guid invitationId,
        int roleId, 
        string shortDescription, 
        string description, 
        int requiredMember,
        DateTime deadLine,
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);

        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        @event.UpdateInvitation(currentUser, invitationId, roleId, shortDescription, description, requiredMember, deadLine);
        
        return await _eventRepository.UpdateEventAggregate(@event, cancellationToken);
    }

    public async Task<bool> DeleteInvitation(
        Guid eventId, 
        Guid invitationId, 
        Guid currentUser, 
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);
        
        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        @event.RemoveInvitation(currentUser, invitationId);
        
        return await _eventRepository.UpdateEventAggregate(@event, cancellationToken);
    }

    public async Task<bool> TakeRequestOnInvitation(
        Guid eventId, 
        Guid invitationId, 
        Guid studentId, 
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);
        
        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        @event.TakeRequestByInvitationId(studentId, invitationId);
        
        return await _eventRepository.UpdateEventAggregate(@event, cancellationToken);
    }
    
    public async Task<bool> AcceptRequestOnInvitation(
        Guid eventId, 
        Guid invitationId, 
        Guid studentId, 
        Guid currentUser, 
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);
        
        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        @event.AcceptRequestByInvitationId(studentId, invitationId, currentUser);
        
        return await _eventRepository.UpdateEventAggregate(@event, cancellationToken);
    }
    
    public async Task<bool> CancelRequestOnInvitation(
        Guid eventId, 
        Guid invitationId, 
        Guid studentId, 
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);
        
        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        @event.CancelRequestByInvitationId(studentId, invitationId);
        
        return await _eventRepository.UpdateEventAggregate(@event, cancellationToken);
    }
    
    public async Task<bool> RejectMemberOnInvitation(
        Guid eventId, 
        Guid invitationId, 
        Guid studentId, 
        Guid currentUser, 
        CancellationToken cancellationToken = default)
    {
        Event? @event = await _eventRepository.GetById(eventId, cancellationToken);
        
        if (@event is null)
            throw new NotFoundException("Событие", eventId);
        
        @event.RejectMemberByInvitationId(studentId, invitationId, currentUser);
        
        return await _eventRepository.UpdateEventAggregate(@event, cancellationToken);
    }

    public async Task<Event> UpdateEvent(
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
        CancellationToken cancellationToken = default)
    {
        Event? eventModel = await _eventRepository.FindAsync(eventId, cancellationToken);

        if (eventModel is null)
            throw new NotFoundException("Событие", eventId);
        
        eventModel.Update(
            currentUser, 
            title, 
            shortDescription, 
            description, 
            dateStart, 
            locationId,
            genreId, 
            typeId,
            minAge);

        await _eventRepository.Update(eventModel, cancellationToken);

        return eventModel;
    }

    public async Task<bool> DeleteEvent(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _eventRepository.Delete(eventId, cancellationToken);
    }
}