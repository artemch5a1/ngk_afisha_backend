using EventService.API.Contracts.Members;
using EventService.API.Extensions;
using EventService.API.Extensions.Mappings;
using EventService.Application.UseCases.MemberUseCases.GetAllMember;
using EventService.Application.UseCases.MemberUseCases.GetAllMemberByAuthor;
using EventService.Application.UseCases.MemberUseCases.GetAllMemberByStudent;
using EventService.Application.UseCases.MemberUseCases.GetMemberById;
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
public class MemberActionController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public MemberActionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetAllMember")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<MemberDto>>> GetAllMember(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0, 
        [FromQuery] int take = 50)
    {
        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Member>> result =
            await _mediator.Send(new GetAllMemberQuery(contract), cancellationToken);

        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetAllMemberByAuthor")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<MemberDto>>> GetAllMemberByAuthor(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0, 
        [FromQuery] int take = 50)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(_ => new List<MemberDto>());
        }
        
        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Member>> result =
            await _mediator.Send(new GetAllMemberByAuthorQuery(userId.Value, contract), cancellationToken);

        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetAllMemberByStudent")]
    [Authorize(Policy = PolicyNames.UserOnly)]
    public async Task<ActionResult<List<MemberDto>>> GetAllMemberByStudent(
        CancellationToken cancellationToken,
        [FromQuery] int skip = 0, 
        [FromQuery] int take = 50)
    {
        Result<Guid> userId = User.ExtractGuid();

        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(_ => new List<MemberDto>());
        }
        
        PaginationContract contract = new PaginationContract(skip, take);

        Result<List<Member>> result =
            await _mediator.Send(new GetAllMemberByStudentQuery(userId.Value, contract), cancellationToken);

        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetMemberById")]
    [Authorize(Policy = PolicyNames.UserOnly)]
    public async Task<ActionResult<MemberDto>> GetMemberById(
        [FromQuery] Guid invitationId,
        [FromQuery] Guid studentId,
        CancellationToken cancellationToken)
    {
        Result<Member> result =
            await _mediator.Send(new GetMemberByIdQuery(studentId, invitationId), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }
}