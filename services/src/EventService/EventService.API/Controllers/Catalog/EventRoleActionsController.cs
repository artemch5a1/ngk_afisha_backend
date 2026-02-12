using EventService.API.Contracts.EventRole;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.UseCases.EventRoleUseCases.DeleteEventRole;
using EventService.Application.UseCases.EventRoleUseCases.GetAllEventRole;
using EventService.Application.UseCases.EventRoleUseCases.GetEventRoleById;
using EventService.Domain.Models;
using EventService.Domain.Result;
using EventService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class EventRoleActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventRoleActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllEventRole")]
    public async Task<ActionResult<List<EventRoleDto>>> GetAllEventRole(
        CancellationToken cancellationToken
    )
    {
        Result<List<EventRole>> result = await _mediator.Send(
            new GetAllEventRoleQuery(null),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetEventRoleById/{roleId:int}")]
    public async Task<ActionResult<EventRoleDto>> GetEventRoleById(
        int roleId,
        CancellationToken cancellationToken
    )
    {
        Result<EventRole> result = await _mediator.Send(
            new GetEventRoleByIdQuery(roleId),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPost("CreateEventRole")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<EventRoleDto>> CreateEventRole(
        [FromBody] CreateEventRoleDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<EventRole> createdLocation = await _mediator.Send(
            dto.ToCommand(),
            cancellationToken
        );

        return createdLocation.ToActionResult(x => x.ToDto());
    }

    [HttpPut("UpdateEventRole")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateEventRole(
        [FromBody] UpdateEventRoleDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<int> result = await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("DeleteEventRole/{roleId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteEventType(
        int roleId,
        CancellationToken cancellationToken
    )
    {
        Result<int> resultDelete = await _mediator.Send(
            new DeleteEventRoleCommand(roleId),
            cancellationToken
        );

        return resultDelete.ToActionResult();
    }
}
