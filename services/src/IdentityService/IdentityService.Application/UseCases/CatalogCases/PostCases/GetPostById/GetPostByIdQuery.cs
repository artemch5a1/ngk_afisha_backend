using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.GetPostById;

public record GetPostByIdQuery(int PostId) : IRequest<Result<Post>>;
