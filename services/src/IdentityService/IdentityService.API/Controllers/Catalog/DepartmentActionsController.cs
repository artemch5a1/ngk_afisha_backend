using IdentityService.API.Contracts.DepartmentActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.CatalogCases.DepartmentCases.DeleteDepartment;
using IdentityService.Application.UseCases.CatalogCases.DepartmentCases.GetAllDepartment;
using IdentityService.Application.UseCases.CatalogCases.DepartmentCases.GetDepartmentById;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class DepartmentActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateDepartment")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto dto, CancellationToken cancellationToken)
    {
        Result<Department> result = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpPut("UpdateDepartment")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdateDepartment([FromBody] UpdateDepartmentDto dto, CancellationToken cancellationToken)
    {
        Result<int> result = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }
    
    [HttpDelete("DeleteDepartment/{departmentId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteDepartment(int departmentId, CancellationToken cancellationToken)
    {
        Result<int> result = 
            await _mediator.Send(new DeleteDepartmentCommand(departmentId), cancellationToken);

        return result.ToActionResult();
    }
    
    [HttpGet("GetDepartmentById/{departmentId:int}")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int departmentId, CancellationToken cancellationToken)
    {
        Result<Department> result = 
            await _mediator.Send(new GetDepartmentByIdQuery(departmentId), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpGet("GetAllDepartment")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<DepartmentDto>>> GetAllSpecialty(CancellationToken cancellationToken)
    {
        Result<List<Department>> result = 
            await _mediator.Send(new GetAllDepartmentQuery(), cancellationToken);

        return result.ToActionResult(x => x.ToListDto());
    }
}