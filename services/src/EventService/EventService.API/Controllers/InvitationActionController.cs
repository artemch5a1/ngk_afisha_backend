using EventService.API.Contracts.Invitation;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.UseCases.InvitationCases.DeleteInvitation;
using EventService.Application.UseCases.InvitationCases.GetAllActualInvitation;
using EventService.Application.UseCases.InvitationCases.GetAllInvitation;
using EventService.Application.UseCases.InvitationCases.GetAllInvitationByAuthor;
using EventService.Application.UseCases.InvitationCases.GetAllInvitationByEvent;
using EventService.Application.UseCases.InvitationCases.GetInvitationById;
using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using EventService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InvitationActionController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationActionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllActualInvitation")]
    public async Task<ActionResult<List<InvitationDto>>> GetAllActualInvitation(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50
    )
    {
        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Invitation>> result = await _mediator.Send(
            new GetAllActualInvitationQuery(contract),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetAllInvitation")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<InvitationDto>>> GetAllInvitation(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50
    )
    {
        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Invitation>> result = await _mediator.Send(
            new GetAllInvitationQuery(contract),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetAllInvitationByAuthor")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<InvitationDto>>> GetAllInvitationByAuthor(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(_ => new List<InvitationDto>());
        }

        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Invitation>> result = await _mediator.Send(
            new GetAllInvitationByAuthorQuery(userId.Value, contract),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetAllInvitationByEvent/{eventId:guid}")]
    public async Task<ActionResult<List<InvitationDto>>> GetAllInvitationByEvent(
        CancellationToken cancellationToken,
        Guid eventId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50
    )
    {
        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Invitation>> result = await _mediator.Send(
            new GetAllInvitationByEventQuery(eventId, contract),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetInvitationById/{invitationId:guid}")]
    public async Task<ActionResult<InvitationDto>> GetInvitationById(
        Guid invitationId,
        CancellationToken cancellationToken
    )
    {
        Result<Invitation> result = await _mediator.Send(
            new GetInvitationByIdQuery(invitationId),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPost("CreateInvitation")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<InvitationDto>> CreateInvitation(
        [FromBody] CreateInvitationDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(_ => new InvitationDto());
        }

        Result<Invitation> result = await _mediator.Send(
            dto.ToCommand(userId.Value),
            cancellationToken
        );

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPost("TakeRequestOnInvitation")]
    [Authorize(Policy = PolicyNames.UserOnly)]
    public async Task<ActionResult<Guid>> TakeRequestOnInvitation(
        [FromBody] TakeRequestOnInvitationDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Guid> result = await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost("CancelRequestOnInvitation")]
    [Authorize(Policy = PolicyNames.UserOnly)]
    public async Task<ActionResult<Guid>> CancelRequestOnInvitation(
        [FromBody] CancelRequestOnInvitationDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Guid> result = await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost("AcceptRequestOnInvitation")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<Guid>> AcceptRequestOnInvitation(
        [FromBody] AcceptRequestOnInvitationDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Guid> result = await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost("RejectMemberOnOnInvitation")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<Guid>> RejectMemberOnOnInvitation(
        [FromBody] RejectMemberOnInvitationDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Guid> result = await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("DeleteInvitation")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<Guid>> DeleteInvitation(
        [FromQuery] Guid eventId,
        [FromQuery] Guid invitationId,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Guid> result = await _mediator.Send(
            new DeleteInvitationCommand(eventId, invitationId, userId.Value),
            cancellationToken
        );

        return result.ToActionResult();
    }

    [HttpPut("UpdateInvitation")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<Guid>> UpdateInvitation(
        UpdateInvitationDto dto,
        CancellationToken cancellationToken
    )
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Guid> result = await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return result.ToActionResult();
    }
}
