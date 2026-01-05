using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.GetAllInvitationByEvent;

public record GetAllInvitationByEventQuery(Guid EventId, PaginationContract? Contract) : IRequest<Result<List<Invitation>>>;