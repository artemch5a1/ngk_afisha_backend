using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.UpdatePost;

public record UpdatePostCommand(int PostId, string PostTitle, int DepartmentId)
    : IRequest<Result<int>>;
