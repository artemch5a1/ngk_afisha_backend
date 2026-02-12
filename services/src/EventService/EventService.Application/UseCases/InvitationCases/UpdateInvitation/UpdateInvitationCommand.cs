using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.UpdateInvitation;

public record UpdateInvitationCommand(
    Guid EventId,
    Guid CurrentUser,
    Guid InvitationId,
    int RoleId,
    string ShortDescription,
    string Description,
    int RequiredMember,
    DateTime DeadLine
) : IRequest<Result<Guid>>;
