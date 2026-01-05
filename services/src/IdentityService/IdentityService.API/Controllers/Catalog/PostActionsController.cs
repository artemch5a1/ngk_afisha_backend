using IdentityService.API.Contracts.PostActions;
using IdentityService.API.Extensions;
using IdentityService.API.Extensions.Mappings;
using IdentityService.Application.UseCases.CatalogCases.PostCases.DeletePost;
using IdentityService.Application.UseCases.CatalogCases.PostCases.GetAllPost;
using IdentityService.Application.UseCases.CatalogCases.PostCases.GetAllPostByDepartment;
using IdentityService.Application.UseCases.CatalogCases.PostCases.GetPostById;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using IdentityService.Infrastructure.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Catalog;

[ApiController]
[Route("[controller]")]
public class PostActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreatePost")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto dto, CancellationToken cancellationToken)
    {
        Result<Post> result = 
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }

    [HttpPut("UpdatePost")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> UpdatePost([FromBody] UpdatePostDto dto, CancellationToken cancellationToken)
    {
        Result<int> result =
            await _mediator.Send(dto.ToCommand(), cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("DeletePost/{postId:int}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<int>> DeleteGroup(int postId, CancellationToken cancellationToken)
    {
        Result<int> result =
            await _mediator.Send(new DeletePostCommand(postId), cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("GetPostById/{postId:int}")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<PostDto>> GetPostById(int postId, CancellationToken cancellationToken)
    {
        Result<Post> result = 
            await _mediator.Send(new GetPostByIdQuery(postId), cancellationToken);

        return result.ToActionResult(x => x.ToDto());
    }
    
    [HttpGet("GetAllPost")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<PostDto>>> GetAllPost(CancellationToken cancellationToken)
    {
        Result<List<Post>> result = 
            await _mediator.Send(new GetAllPostQuery(), cancellationToken);
        
        return result.ToActionResult(x => x.ToListDto());
    }
    
    [HttpGet("GetAllPostByDepartment/{departmentId:int}")]
    [Authorize(Policy = PolicyNames.PublisherOrAdmin)]
    public async Task<ActionResult<List<PostDto>>> GetAllPostByDepartment(int departmentId, CancellationToken cancellationToken)
    {
        Result<List<Post>> result = 
            await _mediator.Send(new GetAllPostByDepartmentQuery(departmentId), cancellationToken);
        
        return result.ToActionResult(x => x.ToListDto());
    }
}