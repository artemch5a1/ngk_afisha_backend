using EventService.Application.Contract;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.UpdateEvent;

public record UpdateEventCommand(
    Guid CurrentUser,
    Guid EventId,
    string Title,
    string ShortDescription,
    string Description,
    DateTime DateStart,
    int LocationId,
    int GenreId,
    int TypeId,
    int MinAge
) : IRequest<Result<UpdatedEvent>>;
