using EventService.Application.Contract;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.CreateEvent;

public record CreateEventCommand(
    string Title, 
    string ShortDescription, 
    string Description, 
    DateTime DateStart,
    int LocationId,
    int GenreId,
    int TypeId,
    int MinAge, 
    Guid Author) : IRequest<Result<CreatedEvent>>;