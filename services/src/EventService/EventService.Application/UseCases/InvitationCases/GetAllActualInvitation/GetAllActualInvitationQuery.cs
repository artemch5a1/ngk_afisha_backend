using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.GetAllActualInvitation;

public record GetAllActualInvitationQuery(PaginationContract? Contract) : IRequest<Result<List<Invitation>>>;