using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.CancelRequestOnEventInvitation;

public record CancelRequestCommand(
    Guid EventId, 
    Guid InvitationId, 
    Guid StudentId) : IRequest<Result<Guid>>;