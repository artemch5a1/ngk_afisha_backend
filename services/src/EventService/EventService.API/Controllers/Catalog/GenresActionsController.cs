using EventService.API.Contracts.Genres;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.UseCases.GenreCases.DeleteGenre;
using EventService.Application.UseCases.GenreCases.GetAllGenre;
using EventService.Application.UseCases.GenreCases.GetGenreById;
using EventService.Domain.Models;
using EventService.Domain.Result;
using EventService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class GenresActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllGenre")]
    public async Task<ActionResult<List<GenreDto>>> GetAllGenre(CancellationToken cancellationToken)
    {
        Result<List<Genre>> result = await _mediator.Send(
            new GetAllGenreQuery(null),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetGenreById/{genreId:int}")]
    public async Task<ActionResult<GenreDto>> GetGenreById(
        int genreId,
        CancellationToken cancellationToken
    )
    {
        Result<Genre> result = await _mediator.Send(
            new GetGenreByIdQuery(genreId),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPost("CreateGenre")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<GenreDto>> CreateGenre(
        [FromBody] CreateGenreDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Genre> createdLocation = await _mediator.Send(dto.ToCommand(), cancellationToken);

        return createdLocation.ToActionResult(x => x.ToDto());
    }

    [HttpPut("UpdateGenre")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateGenre(
        [FromBody] UpdateGenreDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<int> result = await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("DeleteGenre/{genreId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteGenre(
        int genreId,
        CancellationToken cancellationToken
    )
    {
        Result<int> resultDelete = await _mediator.Send(
            new DeleteGenreCommand(genreId),
            cancellationToken
        );

        return resultDelete.ToActionResult();
    }
}
