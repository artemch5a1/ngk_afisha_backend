using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.GetInvitationById;

public record GetInvitationByIdQuery(Guid InvitationId) : IRequest<Result<Invitation>>;
