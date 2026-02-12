using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.DeletePost;

public record DeletePostCommand(int PostId) : IRequest<Result<int>>;
