using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventTypeCases.CreateEventType;

public record CreateEventTypeCommand(string Title) : IRequest<Result<EventType>>;