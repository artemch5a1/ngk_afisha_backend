using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.GenreCases.DeleteGenre;

public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, Result<int>>
{
    private readonly IGenreService _genreService;

    private readonly ILogger<DeleteGenreHandler> _logger;


    public DeleteGenreHandler(IGenreService genreService, ILogger<DeleteGenreHandler> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = 
                await _genreService.DeleteGenre(request.GenreId, cancellationToken);

            return result
                ? Result<int>.Success(request.GenreId)
                : Result<int>.Failure(["Ошибка при удалении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении жанра");

            return Result<int>.Failure(ex);
        }
    }
}