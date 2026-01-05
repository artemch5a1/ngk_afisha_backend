using IdentityService.API.Contracts.AccountActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.Contracts;
using IdentityService.Application.UseCases.AccountCases.GetAccountById;
using IdentityService.Application.UseCases.AccountCases.GetAllAccounts;
using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct
        )
    {
        Result<Guid> accountId = User.ExtractGuid();
        
        Result<LoginResponse> result = await _mediator.Send(request.ToCommand(), ct);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpPost("LoginAdmin")]
    public async Task<ActionResult<LoginResponseDto>> LoginAdmin(
        [FromBody] LoginAdminRequest request,
        CancellationToken ct
    )
    {
        Result<LoginResponse> result = await _mediator.Send(request.ToCommand(), ct);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpPost("RegistryStudent")]
    public async Task<ActionResult<CreatedAccountDto>>  RegistryRegistryStudent
    (
        [FromBody] RegistryStudentDto request, 
        CancellationToken ct
        )
    {
        Result<AccountCreated> result = 
            await _mediator.Send(request.ToCommand(), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpPost("RegistryPublisher")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<CreatedAccountDto>>  RegistryPublisher
    (
        [FromBody] RegistryPublisherDto request, 
        CancellationToken ct
    )
    {
        Result<AccountCreated> result = 
            await _mediator.Send(request.ToCommand(), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpGet("GetAllAccounts")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<List<AccountDto>>>  GetAllUsers(CancellationToken ct)
    {
        Result<List<Account>> result = 
            await _mediator.Send(new GetAllAccountsQuery(), ct);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetAccountById/{accountId:Guid}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<AccountDto>>  GetUserById(Guid accountId, CancellationToken ct)
    {
        Result<Account> result = 
            await _mediator.Send(new GetAccountByIdQuery(accountId), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpGet("CurrentAccount")]
    [Authorize]
    public async Task<ActionResult<AccountDto>>  GetCurrentAccount(CancellationToken ct)
    {
        Result<Guid> accountId = User.ExtractGuid();
        
        if (!accountId.IsSuccess)
        {
            return accountId.ToActionResult(x => new AccountDto());
        }
        
        Result<Account> result = 
            await _mediator.Send(new GetAccountByIdQuery(accountId.Value), ct);
        
        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPatch("ChangePassword")]
    [Authorize]
    public async Task<ActionResult<Guid>> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        Result<Guid> accountId = User.ExtractGuid();
        
        if (!accountId.IsSuccess)
        {
            return accountId.ToActionResult();
        }

        Result<Guid> result = await
            _mediator.Send(dto.ToCommand(accountId.Value), cancellationToken);

        return result.ToActionResult();
    }
}