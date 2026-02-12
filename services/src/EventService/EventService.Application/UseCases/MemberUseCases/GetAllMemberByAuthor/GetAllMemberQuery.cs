using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.MemberUseCases.GetAllMemberByAuthor;

public record GetAllMemberByAuthorQuery(Guid AuthorId, PaginationContract? Contract)
    : IRequest<Result<List<Member>>>;
