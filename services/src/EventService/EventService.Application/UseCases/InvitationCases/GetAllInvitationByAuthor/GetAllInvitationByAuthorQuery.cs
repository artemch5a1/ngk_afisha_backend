using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.GetAllInvitationByAuthor;

public record GetAllInvitationByAuthorQuery(Guid AuthorId, PaginationContract? Contract)
    : IRequest<Result<List<Invitation>>>;
