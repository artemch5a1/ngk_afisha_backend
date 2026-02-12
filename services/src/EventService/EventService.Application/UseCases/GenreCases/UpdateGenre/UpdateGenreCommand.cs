using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.GenreCases.UpdateGenre;

public record UpdateGenreCommand(int GenreId, string Title) : IRequest<Result<int>>;
