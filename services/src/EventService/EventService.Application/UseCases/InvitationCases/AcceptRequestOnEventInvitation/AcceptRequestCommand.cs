using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.AcceptRequestOnEventInvitation;

public record AcceptRequestCommand(
    Guid EventId,
    Guid InvitationId,
    Guid StudentId,
    Guid CurrentUser
) : IRequest<Result<Guid>>;
