using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.GenreCases.DeleteGenre;

public record DeleteGenreCommand(int GenreId) : IRequest<Result<int>>;
