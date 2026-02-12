using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.TakeRequestOnEventInvitation;

public record TakeRequestCommand(Guid EventId, Guid InvitationId, Guid StudentId)
    : IRequest<Result<Guid>>;
