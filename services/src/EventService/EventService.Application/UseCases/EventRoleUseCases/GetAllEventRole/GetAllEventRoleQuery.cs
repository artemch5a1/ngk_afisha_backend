using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.EventRoleUseCases.GetAllEventRole;

public record GetAllEventRoleQuery(PaginationContract? Contract) : IRequest<Result<List<EventRole>>>;