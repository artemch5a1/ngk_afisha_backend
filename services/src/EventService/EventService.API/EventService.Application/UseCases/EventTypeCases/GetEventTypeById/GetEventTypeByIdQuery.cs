using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventTypeCases.GetEventTypeById;

public record GetEventTypeByIdQuery(int EventTypeId) : IRequest<Result<EventType>>;