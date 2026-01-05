using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.GenreCases.CreateGenre;

public record CreateGenreCommand(string Title) : IRequest<Result<Genre>>;