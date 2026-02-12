using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.LocationCases.GetAllLocation;

public record GetAllLocationQuery(PaginationContract? Contract) : IRequest<Result<List<Location>>>;
