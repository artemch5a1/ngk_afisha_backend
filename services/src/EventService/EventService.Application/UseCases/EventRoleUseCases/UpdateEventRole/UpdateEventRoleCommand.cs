using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventRoleUseCases.UpdateEventRole;

public record UpdateEventRoleCommand(int EventRoleId, string Title, string Description)
    : IRequest<Result<int>>;
