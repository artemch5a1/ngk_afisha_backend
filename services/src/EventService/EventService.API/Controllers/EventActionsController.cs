using EventService.API.Contracts.Events;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.Contract;
using EventService.Application.UseCases.EventCases.CreateDefaultEvents;
using EventService.Application.UseCases.EventCases.DeleteEvent;
using EventService.Application.UseCases.EventCases.GetAllEvent;
using EventService.Application.UseCases.EventCases.GetAllEventByAuthor;
using EventService.Application.UseCases.EventCases.GetEventById;
using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Infrastructure.Static;
using EventService.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventActionsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EventActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllEvent")]
    public async Task<ActionResult<List<EventDto>>> GetAllEvent(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        PaginationContract paginationContract = new PaginationContract(skip, take);

        Result<List<Event>> result = 
            await _mediator.Send(new GetAllEventQuery(paginationContract), cancellationToken);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetAllEventByAuthor")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<EventDto>>> GetAllEventByAuthor(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(_ => new List<EventDto>());
        }
        
        PaginationContract paginationContract = new PaginationContract(skip, take);

        Result<List<Event>> result = 
            await _mediator.Send(new GetAllEventByAuthorQuery(userId.Value, paginationContract), cancellationToken);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetEventById/{eventId:guid}")]
    public async Task<ActionResult<EventDto>> GetEventById(
        Guid eventId,
        CancellationToken cancellationToken)
    {
        Result<Event> result = 
            await _mediator.Send(new GetEventByIdQuery(eventId), cancellationToken);
        
        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPost("CreateDefaultEvents")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<List<CreatedEventDto>>> CreateDefaultEvents(CancellationToken cancellationToken)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(x => new List<CreatedEventDto>());
        }
        
        Result<List<CreatedEvent>> createdEvent = 
            await _mediator.Send(new CreateDefaultEventsCommand(userId.Value), cancellationToken);

        return createdEvent
            .ToActionResult(x => x.Select(@event => @event.ToDto()).ToList());
    }
    
    [HttpPost("CreateEvent")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<CreatedEventDto>> CreateEvent([FromBody] CreateEventDto dto, CancellationToken cancellationToken)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(x => new CreatedEventDto());
        }


        Result<CreatedEvent> createdEvent = 
            await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return createdEvent.ToActionResult(x => x.ToDto());
    }
    
    [HttpPut("UpdateEvent")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<UpdatedEventDto>> UpdateEvent([FromBody] UpdateEventDto dto, CancellationToken cancellationToken)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(x => new UpdatedEventDto());
        }


        Result<UpdatedEvent> createdEvent = 
            await _mediator.Send(dto.ToCommand(userId.Value), cancellationToken);

        return createdEvent.ToActionResult(x => x.ToDto());
    }
    
    [HttpDelete("DeleteEvent/{eventId:guid}")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<Guid>> DeleteEvent(Guid eventId, CancellationToken cancellationToken)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }

        Result<Event> result = 
            await _mediator.Send(new GetEventByIdQuery(eventId), cancellationToken);

        if (!result.IsSuccess)
            return result.ToActionResult(_ => eventId);
        
        if(!result.Value!.IsMayDelete(userId.Value))
            return new NotFoundResult();

        Result<Guid> resultDelete = 
            await _mediator.Send(new DeleteEventCommand(eventId), cancellationToken);

        return resultDelete.ToActionResult();
    }
}