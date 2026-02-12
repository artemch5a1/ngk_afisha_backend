using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.RejectMemberOnEventInvitation;

public record RejectMemberCommand(Guid EventId, Guid InvitationId, Guid StudentId, Guid CurrentUser)
    : IRequest<Result<Guid>>;
