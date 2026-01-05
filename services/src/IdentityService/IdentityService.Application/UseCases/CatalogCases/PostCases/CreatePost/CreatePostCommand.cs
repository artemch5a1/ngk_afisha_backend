using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.CreatePost;

public record CreatePostCommand(string PostTitle, int DepartmentId) : IRequest<Result<Post>>;