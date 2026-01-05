using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.GenreCases.GetGenreById;

public record GetGenreByIdQuery(int GenreId) : IRequest<Result<Genre>>;