using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.LocationCases.UpdateLocation;

public record UpdateLocationCommand(int LocationId, string Title, string Address)
    : IRequest<Result<int>>;
