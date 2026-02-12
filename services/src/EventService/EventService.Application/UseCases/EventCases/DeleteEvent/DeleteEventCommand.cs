using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.DeleteEvent;

public record DeleteEventCommand(Guid EventId) : IRequest<Result<Guid>>;
