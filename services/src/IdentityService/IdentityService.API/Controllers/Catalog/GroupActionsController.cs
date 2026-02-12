using IdentityService.API.Contracts.GroupActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.CatalogCases.GroupCases.DeleteGroup;
using IdentityService.Application.UseCases.CatalogCases.GroupCases.GetAllGroup;
using IdentityService.Application.UseCases.CatalogCases.GroupCases.GetAllGroupBySpecialty;
using IdentityService.Application.UseCases.CatalogCases.GroupCases.GetGroupById;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class GroupActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateGroup")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<GroupDto>> CreateGroup(
        [FromBody] CreateGroupDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Group> result = await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPut("UpdateGroup")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateGroup(
        [FromBody] UpdateGroupDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<int> result = await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("DeleteGroup/{groupId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteGroup(
        int groupId,
        CancellationToken cancellationToken
    )
    {
        Result<int> result = await _mediator.Send(
            new DeleteGroupCommand(groupId),
            cancellationToken
        );

        return result.ToActionResult();
    }

    [HttpGet("GetGroupById/{id:int}")]
    public async Task<ActionResult<GroupDto>> GetGroupById(
        int id,
        CancellationToken cancellationToken
    )
    {
        Result<Group> result = await _mediator.Send(new GetGroupByIdQuery(id), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpGet("GetAllGroup")]
    public async Task<ActionResult<List<GroupDto>>> GetAllGroup(CancellationToken cancellationToken)
    {
        Result<List<Group>> result = await _mediator.Send(
            new GetAllGroupQuery(),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetAllGroupBySpecialty/{specialtyId:int}")]
    public async Task<ActionResult<List<GroupDto>>> GetAllGroupBySpecialty(
        int specialtyId,
        CancellationToken cancellationToken
    )
    {
        Result<List<Group>> result = await _mediator.Send(
            new GetAllGroupBySpecialtyQuery(specialtyId),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }
}
