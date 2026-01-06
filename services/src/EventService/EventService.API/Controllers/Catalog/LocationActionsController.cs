using EventService.API.Contracts.Locations;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.UseCases.LocationCases.DeleteLocation;
using EventService.Application.UseCases.LocationCases.GetAllLocation;
using EventService.Application.UseCases.LocationCases.GetLocationById;
using EventService.Domain.Models;
using EventService.Infrastructure.Static;
using EventService.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class LocationActionsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public LocationActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetAllLocation")]
    public async Task<ActionResult<List<LocationDto>>> GetAllLocation(
        CancellationToken cancellationToken)
    {
        Result<List<Location>> result = 
            await _mediator.Send(new GetAllLocationQuery(null), cancellationToken);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetLocationById/{locationId:int}")]
    public async Task<ActionResult<LocationDto>> GetLocationById(
        int locationId,
        CancellationToken cancellationToken)
    {
        Result<Location> result = 
            await _mediator.Send(new GetLocationByIdQuery(locationId), cancellationToken);
        
        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpPost("CreateLocation")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<LocationDto>> CreateLocation([FromBody] CreateLocationDto dto, CancellationToken cancellationToken)
    {
        Result<Location> createdLocation = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return createdLocation.ToActionResult(x => x.ToDto());
    }
    
    [HttpPut("UpdateLocation")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateLocation([FromBody] UpdateLocationDto dto, CancellationToken cancellationToken)
    {
        Result<int> result = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }
    
    [HttpDelete("DeleteLocation/{locationId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteLocation(int locationId, CancellationToken cancellationToken)
    {
        Result<int> resultDelete = 
            await _mediator.Send(new DeleteLocationCommand(locationId), cancellationToken);

        return resultDelete.ToActionResult();
    }
}