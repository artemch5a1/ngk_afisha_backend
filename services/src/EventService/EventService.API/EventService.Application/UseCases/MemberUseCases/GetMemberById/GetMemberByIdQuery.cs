using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.MemberUseCases.GetMemberById;

public record GetMemberByIdQuery(Guid StudentId, Guid InvitationId) : IRequest<Result<Member>>;