using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventRoleUseCases.GetEventRoleById;

public record GetEventRoleByIdQuery(int EventRoleId) : IRequest<Result<EventRole>>;
