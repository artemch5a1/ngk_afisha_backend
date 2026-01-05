using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventTypeCases.DeleteEventType;

public record DeleteEventTypeCommand(int TypeId) : IRequest<Result<int>>;