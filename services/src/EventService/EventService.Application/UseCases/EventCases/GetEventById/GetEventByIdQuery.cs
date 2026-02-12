using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.GetEventById;

public record GetEventByIdQuery(Guid EventId) : IRequest<Result<Event>>;
