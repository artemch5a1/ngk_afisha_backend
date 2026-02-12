using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.LocationCases.CreateLocation;

public record CreateLocationCommand(string Title, string Address) : IRequest<Result<Location>>;
