using IdentityService.API.Contracts.StudentActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.StudentCases.GetAllStudent;
using IdentityService.Application.UseCases.StudentCases.GetStudentById;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetAllStudent")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<List<StudentDto>>>  GetAllUsers(CancellationToken ct)
    {
        Result<List<Student>> result = 
            await _mediator.Send(new GetAllStudentQuery(), ct);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetStudentById/{studentId:Guid}")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<StudentDto>>  GetStudentById(Guid studentId, CancellationToken ct)
    {
        Result<Student> result = 
            await _mediator.Send(new GetStudentByIdQuery(studentId), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpGet("CurrentStudent")]
    [Authorize(Policy = PolicyNames.UserOnly)]
    public async Task<ActionResult<StudentDto>>  GetCurrentStudent(CancellationToken ct)
    {
        Result<Guid> studentId = User.ExtractGuid();
        
        if (!studentId.IsSuccess)
        {
            return studentId.ToActionResult(x => new StudentDto());
        }
        
        Result<Student> result = 
            await _mediator.Send(new GetStudentByIdQuery(studentId.Value), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }
}