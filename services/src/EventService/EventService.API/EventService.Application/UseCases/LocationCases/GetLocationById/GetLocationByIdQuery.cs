using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.LocationCases.GetLocationById;

public record GetLocationByIdQuery(int LocationId) : IRequest<Result<Location>>;