using EventService.API.Contracts.EventTypes;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.UseCases.EventTypeCases.DeleteEventType;
using EventService.Application.UseCases.EventTypeCases.GetAllEventType;
using EventService.Application.UseCases.EventTypeCases.GetEventTypeById;
using EventService.Domain.Models;
using EventService.Domain.Result;
using EventService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class EventTypeActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventTypeActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllEventType")]
    public async Task<ActionResult<List<EventTypeDto>>> GetAllEventType(
        CancellationToken cancellationToken
    )
    {
        Result<List<EventType>> result = await _mediator.Send(
            new GetAllEventTypeQuery(null),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetEventTypeById/{typeId:int}")]
    public async Task<ActionResult<EventTypeDto>> GetGenreById(
        int typeId,
        CancellationToken cancellationToken
    )
    {
        Result<EventType> result = await _mediator.Send(
            new GetEventTypeByIdQuery(typeId),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPost("CreateEventType")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<EventTypeDto>> CreateEventType(
        [FromBody] CreateEventTypeDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<EventType> createdLocation = await _mediator.Send(
            dto.ToCommand(),
            cancellationToken
        );

        return createdLocation.ToActionResult(x => x.ToDto());
    }

    [HttpPut("UpdateEventType")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateEventType(
        [FromBody] UpdateEventTypeDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<int> result = await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("DeleteEventType/{typeId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteEventType(
        int typeId,
        CancellationToken cancellationToken
    )
    {
        Result<int> resultDelete = await _mediator.Send(
            new DeleteEventTypeCommand(typeId),
            cancellationToken
        );

        return resultDelete.ToActionResult();
    }
}
