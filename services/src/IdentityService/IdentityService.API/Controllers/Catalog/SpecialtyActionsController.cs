using IdentityService.API.Contracts.SpecialtyActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.DeleteSpecialty;
using IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.GetAllSpecialty;
using IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.GetSpecialtyById;
using IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.UpdateSpecialty;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class SpecialtyActionsController
{
    private readonly IMediator _mediator;

    public SpecialtyActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateSpecialty")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<SpecialtyDto>> CreateSpecialty([FromBody] CreateSpecialtyDto dto, CancellationToken cancellationToken)
    {
        Result<Specialty> result = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpPut("UpdateSpecialty")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateSpecialty([FromBody] UpdateSpecialtyDto dto, CancellationToken cancellationToken)
    {
        Result<int> result = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }
    
    [HttpDelete("DeleteSpecialty/{specialtyId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteSpecialty(int specialtyId, CancellationToken cancellationToken)
    {
        Result<int> result = 
            await _mediator.Send(new DeleteSpecialtyCommand(specialtyId), cancellationToken);

        return result.ToActionResult();
    }
    
    [HttpGet("GetSpecialtyById/{id:int}")]
    public async Task<ActionResult<SpecialtyDto>> GetSpecialtyById(int id, CancellationToken cancellationToken)
    {
        Result<Specialty> result = 
            await _mediator.Send(new GetSpecialtyByIdQuery(id), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpGet("GetAllSpecialty")]
    public async Task<ActionResult<List<SpecialtyDto>>> GetAllSpecialty(CancellationToken cancellationToken)
    {
        Result<List<Specialty>> result = 
            await _mediator.Send(new GetAllSpecialtyQuery(), cancellationToken);

        return result.ToActionResult(x => x.ToListDto());
    }
}