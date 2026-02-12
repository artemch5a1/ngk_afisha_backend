using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.GenreCases.UpdateGenre;

public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, Result<int>>
{
    private readonly IGenreService _genreService;

    private readonly ILogger<UpdateGenreHandler> _logger;

    public UpdateGenreHandler(IGenreService genreService, ILogger<UpdateGenreHandler> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        UpdateGenreCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _genreService.UpdateGenre(
                request.GenreId,
                request.Title,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.GenreId)
                : Result<int>.Failure(["Ошибка при обновлении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении жанра");

            return Result<int>.Failure(ex);
        }
    }
}
