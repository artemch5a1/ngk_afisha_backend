using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.LocationCases.DeleteLocation;

public record DeleteLocationCommand(int LocationId) : IRequest<Result<int>>;
