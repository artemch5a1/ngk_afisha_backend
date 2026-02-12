using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventTypeCases.UpdateEventType;

public record UpdateEventTypeCommand(int TypeId, string Title) : IRequest<Result<int>>;
