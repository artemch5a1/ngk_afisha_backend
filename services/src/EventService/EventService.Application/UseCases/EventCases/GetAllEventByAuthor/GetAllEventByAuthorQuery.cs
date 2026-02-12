using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventCases.GetAllEventByAuthor;

public record GetAllEventByAuthorQuery(Guid AuthorId, PaginationContract? Contract)
    : IRequest<Result<List<Event>>>;
