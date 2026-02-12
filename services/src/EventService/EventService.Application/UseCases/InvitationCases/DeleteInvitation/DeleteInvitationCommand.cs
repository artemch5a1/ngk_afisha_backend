using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.DeleteInvitation;

public record DeleteInvitationCommand(Guid EventId, Guid InvitationId, Guid CurrentUser)
    : IRequest<Result<Guid>>;
