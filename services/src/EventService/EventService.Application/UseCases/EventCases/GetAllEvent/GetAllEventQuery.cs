using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.GetAllEvent;

public record GetAllEventQuery(PaginationContract? Contract) : IRequest<Result<List<Event>>>;