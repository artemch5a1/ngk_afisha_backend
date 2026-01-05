using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.InvitationCases.CreateInvitation;

public record CreateInvitationCommand(
    Guid EventId, 
    Guid CurrentUser, 
    int RoleId, 
    string ShortDescription, 
    string Description, 
    int RequiredMember, 
    DateTime DeadLine) : IRequest<Result<Invitation>>;