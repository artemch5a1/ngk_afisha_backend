using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.GenreCases.CreateGenre;

public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, Result<Genre>>
{
    private readonly IGenreService _genreService;

    private readonly ILogger<CreateGenreHandler> _logger;

    public CreateGenreHandler(IGenreService genreService, ILogger<CreateGenreHandler> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    public async Task<Result<Genre>> Handle(
        CreateGenreCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Genre location = await _genreService.CreateGenre(request.Title, cancellationToken);

            return Result<Genre>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании жанра");

            return Result<Genre>.Failure(ex);
        }
    }
}
