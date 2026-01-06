using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.GenreCases.GetAllGenre;

public class GetAllGenreHandler : IRequestHandler<GetAllGenreQuery, Result<List<Genre>>>
{
    private readonly IGenreService _genreService;

    private readonly ILogger<GetAllGenreHandler> _logger;


    public GetAllGenreHandler(IGenreService genreService, ILogger<GetAllGenreHandler> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }


    public async Task<Result<List<Genre>>> Handle(GetAllGenreQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Genre> location = 
                await _genreService.GetAllGenre(request.Contract, cancellationToken);

            return Result<List<Genre>>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,"Ошбика при получении всех жанров");

            return Result<List<Genre>>.Failure(ex);
        }
    }
}