using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventRoleUseCases.CreateEventRole;

public record CreateEventRoleCommand(string Title, string Description) : IRequest<Result<EventRole>>;