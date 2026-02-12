using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventRoleUseCases.DeleteEventRole;

public record DeleteEventRoleCommand(int EventRoleId) : IRequest<Result<int>>;
