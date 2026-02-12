using EventService.Application.Contract;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.CreateDefaultEvents;

public record CreateDefaultEventsCommand(Guid AuthorId) : IRequest<Result<List<CreatedEvent>>>;
