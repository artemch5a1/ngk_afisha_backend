using IdentityService.API.Contracts.PublisherActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.PublisherCases.GetAllPublisher;
using IdentityService.Application.UseCases.PublisherCases.GetPublisherById;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PublisherActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublisherActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllPublisher")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<List<PublisherDto>>> GetAllPublisher(CancellationToken ct)
    {
        Result<List<Publisher>> result = await _mediator.Send(new GetAllPublisherQuery(), ct);

        return result.ToActionResult(x => x.ToListDto());
    }

    [HttpGet("GetPublisherById/{publisherId:Guid}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<PublisherDto>> GetPublisherById(
        Guid publisherId,
        CancellationToken ct
    )
    {
        Result<Publisher> result = await _mediator.Send(new GetPublisherByIdQuery(publisherId), ct);

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpGet("CurrentPublisher")]
    [Authorize(Policy = PolicyNames.PublisherOnly)]
    public async Task<ActionResult<PublisherDto>> GetCurrentPublisher(CancellationToken ct)
    {
        Result<Guid> studentId = User.ExtractGuid();

        if (!studentId.IsSuccess)
        {
            return studentId.ToActionResult(x => new PublisherDto());
        }

        Result<Publisher> result = await _mediator.Send(
            new GetPublisherByIdQuery(studentId.Value),
            ct
        );

        return result.ToActionResult(x => x.ToDto());
    }
}
