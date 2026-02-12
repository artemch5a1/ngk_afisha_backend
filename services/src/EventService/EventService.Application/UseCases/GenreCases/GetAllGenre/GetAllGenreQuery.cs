using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.GenreCases.GetAllGenre;

public record GetAllGenreQuery(PaginationContract? Contract) : IRequest<Result<List<Genre>>>;
