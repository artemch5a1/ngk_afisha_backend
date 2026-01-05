using IdentityService.API.Contracts.UserActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.UserCases.GetAllUsers;
using IdentityService.Application.UseCases.UserCases.GetUserById;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetAllUsers")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<List<UserDto>>>  GetAllUsers(CancellationToken ct)
    {
        Result<List<User>> result = 
            await _mediator.Send(new GetAllUsersQuery(), ct);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetUserById/{userId:Guid}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<UserDto>>  GetUserById(Guid userId, CancellationToken ct)
    {
        Result<User> result = 
            await _mediator.Send(new GetUserByIdQuery(userId), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }

    [HttpGet("CurrentUser")]
    [Authorize]
    public async Task<ActionResult<UserDto>>  GetCurrentUser(CancellationToken ct)
    {
        Result<Guid> userId = User.ExtractGuid();
        
        if (!userId.IsSuccess)
        {
            return userId.ToActionResult(_ => new UserDto());
        }
        
        Result<User> result = 
            await _mediator.Send(new GetUserByIdQuery(userId.Value), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }
    
    
    [HttpPut("UpdateUserInfo")]
    [Authorize]
    public async Task<ActionResult<Guid>> UpdateUserInfo(UpdateUserDto dto, CancellationToken ct)
    {
        Result<Guid> userId = User.ExtractGuid();
        
        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }
        
        Result<Guid> result =
            await _mediator.Send(dto.ToCommand(userId.Value), ct);

        return result.ToActionResult();
    }
    
    [HttpPut("UpdateStudentProfile")]
    [Authorize]
    public async Task<ActionResult<Guid>> UpdateStudentProfile(UpdateStudentProfileDto dto, CancellationToken ct)
    {
        Result<Guid> userId = User.ExtractGuid();
        
        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }
        
        Result<Guid> result =
            await _mediator.Send(dto.ToCommand(userId.Value), ct);

        return result.ToActionResult();
    }
    
    [HttpPut("UpdatePublisherProfile")]
    [Authorize]
    public async Task<ActionResult<Guid>> UpdatePublisherProfile(UpdatePublisherProfileDto dto, CancellationToken ct)
    {
        Result<Guid> userId = User.ExtractGuid();
        
        if (!userId.IsSuccess)
        {
            return userId.ToActionResult();
        }
        
        Result<Guid> result =
            await _mediator.Send(dto.ToCommand(userId.Value), ct);

        return result.ToActionResult();
    }
}