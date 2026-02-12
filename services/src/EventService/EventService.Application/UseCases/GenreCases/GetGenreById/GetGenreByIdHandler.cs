using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.GenreCases.GetGenreById;

public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, Result<Genre>>
{
    private readonly IGenreService _genreService;

    private readonly ILogger<GetGenreByIdHandler> _logger;

    public GetGenreByIdHandler(IGenreService genreService, ILogger<GetGenreByIdHandler> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    public async Task<Result<Genre>> Handle(
        GetGenreByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Genre? location = await _genreService.GetGenreById(request.GenreId, cancellationToken);

            if (location is null)
                return Result<Genre>.Failure(["Жанр не найден"], ApiErrorType.NotFound);

            return Result<Genre>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении жанра по id");

            return Result<Genre>.Failure(ex);
        }
    }
}
