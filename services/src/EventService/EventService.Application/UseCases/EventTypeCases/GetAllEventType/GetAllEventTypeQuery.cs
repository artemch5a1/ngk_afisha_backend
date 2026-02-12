using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventTypeCases.GetAllEventType;

public record GetAllEventTypeQuery(PaginationContract? Contract)
    : IRequest<Result<List<EventType>>>;
